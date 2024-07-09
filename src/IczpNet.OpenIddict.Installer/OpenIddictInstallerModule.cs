using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class OpenIddictInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<OpenIddictInstallerModule>();
        });
    }
}
