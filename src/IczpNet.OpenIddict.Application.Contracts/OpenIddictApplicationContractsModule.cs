using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(OpenIddictDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class OpenIddictApplicationContractsModule : AbpModule
{

}
