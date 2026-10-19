using IczpNet.AbpCommons;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.OpenIddict;
using OpenIddict.Core;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.OpenIddict.Authorizations;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(OpenIddictDomainSharedModule)
)]
[DependsOn(typeof(AbpOpenIddictDomainModule))]
[DependsOn(typeof(AbpPermissionManagementDomainOpenIddictModule))]

[DependsOn(typeof(AbpCommonsDomainModule))]
public class OpenIddictDomainModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
        => PreConfigure<OpenIddictCoreBuilder>(builder =>
        {
            builder.ReplaceTokenManager<OpenIddictTokenModel, Tokens.TokenManager>();
            builder.ReplaceAuthorizationManager<OpenIddictAuthorizationModel, Authorizations.AuthorizationManager>();
        });
}
