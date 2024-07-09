using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace IczpNet.OpenIddict.MongoDB;

[DependsOn(
    typeof(OpenIddictTestBaseModule),
    typeof(OpenIddictMongoDbModule)
    )]
public class OpenIddictMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
