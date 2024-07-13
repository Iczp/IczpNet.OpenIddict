using IczpNet.OpenIddict.Applications.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.OpenIddict.Applications;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationDtoResolver : DomainService, IApplicationDtoResolver
{
    public IRepository<OpenIddictApplication, Guid> Repository { get; set; }

    protected Type ObjectMapperContext { get; set; }
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
        ObjectMapperContext == null
            ? provider.GetRequiredService<IObjectMapper>()
            : (IObjectMapper)provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));


    public ApplicationDtoResolver() { }


    public ApplicationSimpleDto GetDto(Guid? appId)
    {
        if (!appId.HasValue)
        {
            return null;
        }

        var application = Repository.FindAsync(appId.Value).Result;

        return ObjectMapper.Map<OpenIddictApplication, ApplicationSimpleDto>(application);

    }

    public async Task<ApplicationSimpleDto> GetDtoAsync(Guid? appId)
    {
        if (!appId.HasValue)
        {
            return null;
        }

        var application = await Repository.FindAsync(appId.Value);

        return ObjectMapper.Map<OpenIddictApplication, ApplicationSimpleDto>(application);
    }
}
