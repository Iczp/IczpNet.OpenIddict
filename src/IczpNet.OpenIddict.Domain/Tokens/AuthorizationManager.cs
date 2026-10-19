using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Tokens;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.Uow;

namespace IczpNet.OpenIddict.Tokens;

public partial class TokenManager : AbpTokenManager, ITokenManager
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; }
    private IDistributedCache ValidationCache => LazyServiceProvider.LazyGetRequiredService<IDistributedCache>();
    private IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    public virtual async Task<bool> IsTokenValidAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        // jti is a different identifier. OpenIddict embeds its database ID in oi_tkn_id.
        var identifier = principal?.GetTokenId();
        if (!Guid.TryParse(identifier, out var id)) return false;
        // Authority-only validation for refresh/introspection. Resource APIs use
        // ITokenRevocationChecker and never resolve this database-backed manager.
        bool valid;
        // Read the store, not the per-process OpenIddict entity cache. Authentication
        // can run before the request UOW, and must observe commits from other hosts.
        using (var unit = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            var entry = await Store.FindByIdAsync(id.ToString("D"), cancellationToken);
            valid = entry != null && await Store.GetStatusAsync(entry, cancellationToken) == OpenIddictConstants.Statuses.Valid;
            var authorizationId = entry == null ? null : await Store.GetAuthorizationIdAsync(entry, cancellationToken);
            if (valid && !string.IsNullOrEmpty(authorizationId))
            {
                var authorizations = LazyServiceProvider.LazyGetRequiredService<IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel>>();
                var authorization = await authorizations.FindByIdAsync(authorizationId, cancellationToken);
                valid = authorization != null && await authorizations.GetStatusAsync(authorization, cancellationToken) == OpenIddictConstants.Statuses.Valid;
            }
            await unit.CompleteAsync(cancellationToken);
        }
        return valid;
    }

    public override ValueTask<bool> TryRevokeAsync(OpenIddictTokenModel token, CancellationToken cancellationToken = default)
        => MutateRevocationAsync(() => base.TryRevokeAsync(token, cancellationToken),
            new TokenRevokedEto { TokenId = token.Id.ToString("D") }, cancellationToken);

    public override ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default)
        => MutateRevocationAsync(() => base.RevokeBySubjectAsync(subject, cancellationToken),
            new TokenRevokedEto { Subject = subject }, cancellationToken);

    public override ValueTask<long> RevokeByAuthorizationIdAsync(string identifier, CancellationToken cancellationToken = default)
        => MutateRevocationAsync(() => base.RevokeByAuthorizationIdAsync(identifier, cancellationToken),
            new TokenRevokedEto { AuthorizationId = identifier }, cancellationToken);

    public override ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken = default)
        => MutateRevocationAsync(() => base.RevokeByApplicationIdAsync(identifier, cancellationToken),
            new TokenRevokedEto { All = true }, cancellationToken);

    public override ValueTask<long> RevokeAsync(string subject, string client, string status, string type, CancellationToken cancellationToken = default)
        => MutateRevocationAsync(() => base.RevokeAsync(subject, client, status, type, cancellationToken),
            new TokenRevokedEto { Subject = subject }, cancellationToken);

    public TokenManager(
        IOpenIddictTokenCache<OpenIddictTokenModel> cache,
        ILogger<OpenIddictTokenManager<OpenIddictTokenModel>> logger,
        IOptionsMonitor<OpenIddictCoreOptions> options,
        IOpenIddictTokenStore<OpenIddictTokenModel> store,
        AbpOpenIddictIdentifierConverter identifierConverter) : base(cache, logger, options, store, identifierConverter)
    {
    }


}
