using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Authorizations;
using System.Threading;
using System.Threading.Tasks;
using IczpNet.OpenIddict.Tokens;

namespace IczpNet.OpenIddict.Authorizations;

public class AuthorizationManager : AbpAuthorizationManager, IAuthorizationManager
{


    private readonly TokenManager tokens;

    public override ValueTask<bool> TryRevokeAsync(OpenIddictAuthorizationModel authorization, CancellationToken cancellationToken = default)
        => tokens.MutateRevocationAsync(() => base.TryRevokeAsync(authorization, cancellationToken),
            new TokenRevokedEto { AuthorizationId = authorization.Id.ToString("D") }, cancellationToken);

    public override ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default)
        => tokens.MutateRevocationAsync(() => base.RevokeBySubjectAsync(subject, cancellationToken),
            new TokenRevokedEto { Subject = subject }, cancellationToken);

    public override ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken = default)
        => tokens.MutateRevocationAsync(() => base.RevokeByApplicationIdAsync(identifier, cancellationToken),
            new TokenRevokedEto { All = true }, cancellationToken);

    public override ValueTask<long> RevokeAsync(string subject, string client, string status, string type, CancellationToken cancellationToken = default)
        => tokens.MutateRevocationAsync(() => base.RevokeAsync(subject, client, status, type, cancellationToken),
            new TokenRevokedEto { Subject = subject }, cancellationToken);
    public AuthorizationManager(
        IOpenIddictAuthorizationCache<OpenIddictAuthorizationModel> cache,
        ILogger<OpenIddictAuthorizationManager<OpenIddictAuthorizationModel>> logger,
        IOptionsMonitor<OpenIddictCoreOptions> options,
        IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel> store,
        AbpOpenIddictIdentifierConverter identifierConverter,
        TokenManager tokens) : base(cache, logger, options, store, identifierConverter)
    {
        this.tokens = tokens;
    }


}
