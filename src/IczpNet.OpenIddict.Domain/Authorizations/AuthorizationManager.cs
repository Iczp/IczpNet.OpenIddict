using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Authorizations;

namespace IczpNet.OpenIddict.Authorizations;

public class AuthorizationManager : AbpAuthorizationManager, IAuthorizationManager
{


    protected IOpenIddictAuthorizationManager OpenIddictAuthorizationManager { get; set; }
    public AuthorizationManager(
        IOpenIddictAuthorizationCache<OpenIddictAuthorizationModel> cache,
        ILogger<OpenIddictAuthorizationManager<OpenIddictAuthorizationModel>> logger,
        IOptionsMonitor<OpenIddictCoreOptions> options,
        IOpenIddictAuthorizationStoreResolver resolver,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IOpenIddictAuthorizationManager openIddictAuthorizationManager) : base(cache, logger, options, resolver, identifierConverter)
    {
        OpenIddictAuthorizationManager = openIddictAuthorizationManager;
    }


}
