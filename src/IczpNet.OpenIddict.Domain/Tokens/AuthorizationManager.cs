using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Tokens;

namespace IczpNet.OpenIddict.Tokens;

public class TokenManager : AbpTokenManager, ITokenManager
{

    protected IOpenIddictTokenManager OpenIddictTokenManager { get; set; }
    public TokenManager(
        IOpenIddictTokenCache<OpenIddictTokenModel> cache,
        ILogger<OpenIddictTokenManager<OpenIddictTokenModel>> logger,
        IOptionsMonitor<OpenIddictCoreOptions> options,
        IOpenIddictTokenStoreResolver resolver,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IOpenIddictTokenManager openIddictTokenManager) : base(cache, logger, options, resolver, identifierConverter)
    {
        OpenIddictTokenManager = openIddictTokenManager;
    }


}
