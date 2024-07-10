using Localization.Resources.AbpUi;
using IczpNet.OpenIddict.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using IczpNet.AbpCommons;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(OpenIddictApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]



[DependsOn(typeof(AbpCommonsHttpApiModule))]
public class OpenIddictHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(OpenIddictHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<OpenIddictResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
