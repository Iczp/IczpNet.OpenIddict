using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OpenIddictHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class OpenIddictConsoleApiClientModule : AbpModule
{

}
