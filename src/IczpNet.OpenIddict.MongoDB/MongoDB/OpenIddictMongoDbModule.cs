using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.MongoDB;

namespace IczpNet.OpenIddict.MongoDB;

[DependsOn(
    typeof(OpenIddictDomainModule),
    typeof(AbpMongoDbModule)
    )]
[DependsOn(typeof(AbpOpenIddictMongoDbModule))]
    public class OpenIddictMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<OpenIddictMongoDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
