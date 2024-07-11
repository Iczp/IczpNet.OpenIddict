using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using IczpNet.AbpCommons;
using Volo.Abp.FluentValidation;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(OpenIddictDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
[DependsOn(typeof(AbpCommonsApplicationContractsModule))]
[DependsOn(typeof(AbpFluentValidationModule))]
    public class OpenIddictApplicationContractsModule : AbpModule
{

}
