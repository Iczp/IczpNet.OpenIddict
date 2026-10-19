using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace IczpNet.OpenIddict;

[DependsOn(typeof(OpenIddictDomainSharedModule), typeof(AbpCachingModule), typeof(AbpAspNetCoreMvcModule))]
public class OpenIddictAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
        => context.Services.AddTransient<IPostConfigureOptions<JwtBearerOptions>, TokenRevocationJwtOptions>();
}
