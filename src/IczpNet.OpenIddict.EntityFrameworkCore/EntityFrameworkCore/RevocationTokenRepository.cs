using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IczpNet.OpenIddict.Tokens;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Tokens;
using AuthDb = Volo.Abp.OpenIddict.EntityFrameworkCore.IOpenIddictDbContext;

namespace IczpNet.OpenIddict.EntityFrameworkCore;

public class RevocationTokenRepository(IDbContextProvider<AuthDb> contexts)
    : EfCoreOpenIddictTokenRepository(contexts), ITokenRevocationRepository
{
    public async Task<List<RevokedTokenState>> ReadTokensAsync(Guid? after, int count, string subject = null, Guid? authorizationId = null, CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-5);
        return await (await GetQueryableAsync()).AsNoTracking()
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid && (!x.ExpirationDate.HasValue || x.ExpirationDate >= cutoff))
            .WhereIf(after.HasValue, x => x.Id.CompareTo(after.Value) > 0)
            .WhereIf(subject != null, x => x.Subject == subject)
            .WhereIf(authorizationId.HasValue, x => x.AuthorizationId == authorizationId)
            .OrderBy(x => x.Id).Take(Math.Clamp(count, 1, 200))
            .Select(x => new RevokedTokenState(x.Id, x.ExpirationDate)).ToListAsync(cancellationToken);
    }

    public async Task<List<Guid>> ReadAuthorizationsAsync(Guid? after, int count, string subject = null, CancellationToken cancellationToken = default)
        => await (await GetDbContextAsync()).Set<OpenIddictAuthorization>().AsNoTracking()
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid)
            .WhereIf(subject != null, x => x.Subject == subject)
            .WhereIf(after.HasValue, x => x.Id.CompareTo(after.Value) > 0)
            .OrderBy(x => x.Id).Take(Math.Clamp(count, 1, 200)).Select(x => x.Id).ToListAsync(cancellationToken);

    public async Task<bool> HasUnexpiredTokensAsync(Guid? authorizationId = null, Guid? applicationId = null, CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-5);
        return await (await GetQueryableAsync()).AnyAsync(x =>
            (!authorizationId.HasValue || x.AuthorizationId == authorizationId) &&
            (!applicationId.HasValue || x.ApplicationId == applicationId) &&
            (!x.ExpirationDate.HasValue || x.ExpirationDate >= cutoff), cancellationToken);
    }

    // Invalid but still usable credentials remain the durable source for Redis recovery.
    public override async Task<long> PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-5);
        var tokens = await GetQueryableAsync();
        long count = 0;
        while (true)
        {
            var ids = await tokens.Where(x => x.CreationDate < date && x.ExpirationDate < cutoff)
                .OrderBy(x => x.Id).Take(200).Select(x => x.Id).ToListAsync(cancellationToken);
            if (ids.Count == 0) return count;
            count += await tokens.Where(x => ids.Contains(x.Id) && x.ExpirationDate < cutoff).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
