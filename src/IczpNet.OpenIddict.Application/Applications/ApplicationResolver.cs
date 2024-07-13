using AutoMapper;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.Tokens.Dtos;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.OpenIddict.Tokens;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationResolver : DomainService, IValueResolver<OpenIddictToken, TokenDto, ApplicationSimpleDto>, ITransientDependency
{

    public IApplicationDtoResolver ApplicationDtoResolver { get; set; }

    protected virtual ApplicationSimpleDto GetAppDto(Guid? appId)
    {
        return ApplicationDtoResolver.GetDto(appId);
    }

    public ApplicationResolver() { }

    public virtual ApplicationSimpleDto Resolve(OpenIddictToken source, TokenDto destination, ApplicationSimpleDto destMember, ResolutionContext context)
    {
        return GetAppDto(source.ApplicationId);
    }
}
