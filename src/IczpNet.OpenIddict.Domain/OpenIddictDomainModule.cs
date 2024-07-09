using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(OpenIddictDomainSharedModule)
)]
public class OpenIddictDomainModule : AbpModule
{

}
