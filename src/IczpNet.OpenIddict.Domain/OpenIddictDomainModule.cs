using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.OpenIddict;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(OpenIddictDomainSharedModule)
)]
[DependsOn(typeof(AbpOpenIddictDomainModule))]
    [DependsOn(typeof(AbpPermissionManagementDomainOpenIddictModule))]
    public class OpenIddictDomainModule : AbpModule
{

}
