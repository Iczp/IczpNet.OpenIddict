using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using IczpNet.AbpCommons;

namespace IczpNet.OpenIddict;

[DependsOn(
    typeof(OpenIddictDomainModule),
    typeof(OpenIddictApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]

[DependsOn(typeof(AbpCommonsApplicationModule))]
public class OpenIddictApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<OpenIddictApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OpenIddictApplicationModule>(validate: true);
        });
    }
}
