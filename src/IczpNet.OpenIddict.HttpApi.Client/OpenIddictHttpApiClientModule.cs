using IczpNet.AbpCommons;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(OpenIddictApplicationContractsModule),
    typeof(AbpHttpClientModule))]


[DependsOn(typeof(AbpCommonsHttpApiClientModule))]
public class OpenIddictHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(OpenIddictApplicationContractsModule).Assembly,
            OpenIddictRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<OpenIddictHttpApiClientModule>();
        });

    }
}
