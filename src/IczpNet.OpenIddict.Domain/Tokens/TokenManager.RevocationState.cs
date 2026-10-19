using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Uow;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Authorizations;

namespace IczpNet.OpenIddict.Tokens;

public partial class TokenManager
{
    private ITokenRevocationRepository Revocations => LazyServiceProvider.LazyGetRequiredService<ITokenRevocationRepository>();
    private IAbpDistributedLock RevocationLock => LazyServiceProvider.LazyGetRequiredService<IAbpDistributedLock>();

    public virtual async Task EnsureRevocationStateAsync(bool force = false, CancellationToken cancellationToken = default)
    {
        if (!force && await ValidationCache.GetStringAsync(TokenRevocationKeys.Ready, cancellationToken) == TokenRevocationKeys.ReadyValue) return;
        if (RevocationLock is NullAbpDistributedLock) throw new AbpException("Revocation recovery requires a real distributed lock.");
        await using var handle = await RevocationLock.TryAcquireAsync(TokenRevocationKeys.MutationLock, TimeSpan.FromSeconds(30), cancellationToken)
            ?? throw new TokenRevocationStateUnavailableException();
        if (!force && await ValidationCache.GetStringAsync(TokenRevocationKeys.Ready, cancellationToken) == TokenRevocationKeys.ReadyValue) return;
        await ValidationCache.RemoveAsync(TokenRevocationKeys.Ready, cancellationToken);
        // Recovery only adds immutable denial markers. It cannot resurrect an earlier revocation.
        await ReplayTokensAsync(null, cancellationToken);
        await ReplayAuthorizationsAsync(null, cancellationToken);
        await ValidationCache.SetStringAsync(TokenRevocationKeys.Ready, TokenRevocationKeys.ReadyValue, cancellationToken);
    }

    private async Task ReplayAuthorizationsAsync(string subject, CancellationToken cancellationToken)
    {
        Guid? after = null;
        while (true)
        {
            List<Guid> page;
            using (var unit = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
            {
                page = await Revocations.ReadAuthorizationsAsync(after, 200, subject, cancellationToken);
                await unit.CompleteAsync(cancellationToken);
            }
            await Task.WhenAll(page.Select(id => ValidationCache.SetStringAsync(TokenRevocationKeys.Authorization(id.ToString("D")), "revoked", cancellationToken)));
            if (page.Count < 200) break;
            after = page[^1];
        }
    }

    private async Task ReplayTokensAsync(string subject, CancellationToken cancellationToken, Guid? authorizationId = null)
    {
        Guid? after = null;
        while (true)
        {
            List<RevokedTokenState> page;
            using (var unit = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
            {
                page = await Revocations.ReadTokensAsync(after, 200, subject, authorizationId, cancellationToken);
                await unit.CompleteAsync(cancellationToken);
            }
            await Task.WhenAll(page.Select(row => RecordTokenAsync(row, cancellationToken)));
            if (page.Count < 200) break;
            after = page[^1].Id;
        }
    }

    private Task RecordTokenAsync(RevokedTokenState token, CancellationToken cancellationToken)
    {
        var options = new DistributedCacheEntryOptions();
        if (token.ExpirationDate.HasValue)
        {
            var expiration = new DateTimeOffset(DateTime.SpecifyKind(token.ExpirationDate.Value, DateTimeKind.Utc)).AddMinutes(5);
            if (expiration <= DateTimeOffset.UtcNow) return Task.CompletedTask;
            options.AbsoluteExpiration = expiration;
        }
        return ValidationCache.SetStringAsync(TokenRevocationKeys.Token(token.Id.ToString("D")), "revoked", options, cancellationToken);
    }

    private sealed class RevocationMutation(IAbpDistributedLockHandle handle, bool wasReady)
    {
        public IAbpDistributedLockHandle Handle = handle;
        public bool WasReady { get; } = wasReady;
        public List<TokenRevokedEto> Notices { get; } = [];
        public async ValueTask ReleaseAsync()
        {
            var held = Interlocked.Exchange(ref Handle, null);
            if (held != null) await held.DisposeAsync();
        }
    }

    internal async ValueTask<T> MutateRevocationAsync<T>(Func<ValueTask<T>> mutation, TokenRevokedEto notice, CancellationToken cancellationToken)
    {
        var unit = UnitOfWorkManager.Current;
        RevocationMutation state;
        if (unit?.Items.TryGetValue(TokenRevocationKeys.MutationLock, out var existing) == true)
            state = (RevocationMutation)existing;
        else
        {
            if (RevocationLock is NullAbpDistributedLock) throw new AbpException("Revocation writes require a real distributed lock.");
            var handle = await RevocationLock.TryAcquireAsync(TokenRevocationKeys.MutationLock, TimeSpan.FromSeconds(30), cancellationToken)
                ?? throw new TokenRevocationStateUnavailableException();
            try
            {
                var wasReady = await ValidationCache.GetStringAsync(TokenRevocationKeys.Ready, cancellationToken) == TokenRevocationKeys.ReadyValue;
                // Close the SQL-commit/Redis-write crash window before changing durable state.
                await ValidationCache.RemoveAsync(TokenRevocationKeys.Ready, cancellationToken);
                state = new RevocationMutation(handle, wasReady);
                if (unit != null)
                {
                    unit.Items[TokenRevocationKeys.MutationLock] = state;
                    unit.OnCompleted(() => CompleteRevocationAsync(state));
                    // ABP exposes a synchronous disposal event. On rollback only, release
                    // the distributed lease here; recovery will rebuild the missing ready marker.
                    unit.Disposed += (_, _) => state.ReleaseAsync().AsTask().GetAwaiter().GetResult();
                }
            }
            catch { await handle.DisposeAsync(); throw; }
        }
        try
        {
            var result = await mutation();
            if (result is not bool succeeded || succeeded) state.Notices.Add(notice);
            if (unit == null) await CompleteRevocationAsync(state);
            return result;
        }
        catch
        {
            if (unit == null) await state.ReleaseAsync();
            throw;
        }
    }

    private async Task CompleteRevocationAsync(RevocationMutation state)
    {
        try
        {
            foreach (var notice in state.Notices) await SynchronizeRevocationAsync(notice);
            if (state.WasReady)
                await ValidationCache.SetStringAsync(TokenRevocationKeys.Ready, TokenRevocationKeys.ReadyValue);
        }
        finally { await state.ReleaseAsync(); }
        // Publish only after DB commit and Redis synchronization. API/Hub authorization
        // remains authoritative when immediate client notification is delayed.
        using var unit = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
        foreach (var notice in state.Notices)
            await LazyServiceProvider.LazyGetRequiredService<Volo.Abp.EventBus.Distributed.IDistributedEventBus>().PublishAsync(notice);
        await unit.CompleteAsync();
    }

    private async Task SynchronizeRevocationAsync(TokenRevokedEto notice)
    {
        if (notice.TokenId != null)
        {
            using var unit = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
            var token = await Store.FindByIdAsync(notice.TokenId, default);
            if (token != null && token.Status != OpenIddictConstants.Statuses.Valid)
                await RecordTokenAsync(new(token.Id, token.ExpirationDate), default);
            await unit.CompleteAsync();
        }
        else if (notice.AuthorizationId != null)
        {
            using var unit = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
            var store = LazyServiceProvider.LazyGetRequiredService<IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel>>();
            var authorization = await store.FindByIdAsync(notice.AuthorizationId, default);
            if (authorization != null && await store.GetStatusAsync(authorization, default) != OpenIddictConstants.Statuses.Valid)
                await ValidationCache.SetStringAsync(TokenRevocationKeys.Authorization(notice.AuthorizationId), "revoked");
            await unit.CompleteAsync();
            // Revoking the currently linked tokens need not revoke the authorization itself.
            await ReplayTokensAsync(null, default, Guid.Parse(notice.AuthorizationId));
        }
        else
        {
            await ReplayTokensAsync(notice.Subject, default);
            await ReplayAuthorizationsAsync(notice.Subject, default);
        }
    }
}
