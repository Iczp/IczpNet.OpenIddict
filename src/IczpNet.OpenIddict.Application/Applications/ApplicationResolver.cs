using AutoMapper;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.Tokens.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Tokens;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationResolver : DomainService, IValueResolver<OpenIddictToken, TokenDto, ApplicationSimpleDto>, ITransientDependency
{
    public IRepository<OpenIddictApplication, Guid> Repository { get; set; }

    protected Type ObjectMapperContext { get; set; }
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
        ObjectMapperContext == null
            ? provider.GetRequiredService<IObjectMapper>()
            : (IObjectMapper)provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));


    public ApplicationResolver() { }

    public ApplicationSimpleDto Resolve(OpenIddictToken source, TokenDto destination, ApplicationSimpleDto destMember, ResolutionContext context)
    {
        var application = Repository.FindAsync(source.ApplicationId.Value).Result;

        return ObjectMapper.Map<OpenIddictApplication, ApplicationSimpleDto>(application);
    }
}
