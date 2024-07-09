using Volo.Abp.Modularity;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(OpenIddictApplicationModule),
    typeof(OpenIddictDomainTestModule)
    )]
public class OpenIddictApplicationTestModule : AbpModule
{

}
