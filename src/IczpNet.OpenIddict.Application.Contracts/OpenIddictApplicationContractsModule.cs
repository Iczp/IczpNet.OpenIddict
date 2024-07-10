using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using IczpNet.AbpCommons;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(OpenIddictDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
[DependsOn(typeof(AbpCommonsApplicationContractsModule))]
public class OpenIddictApplicationContractsModule : AbpModule
{

}
