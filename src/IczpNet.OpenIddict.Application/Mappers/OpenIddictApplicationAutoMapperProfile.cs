using AutoMapper;
using IczpNet.OpenIddict.Applications;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.Tokens.Dtos;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Tokens;

namespace IczpNet.OpenIddict.Mappers;

public class OpenIddictApplicationAutoMapperProfile : Profile
{
    public OpenIddictApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<OpenIddictApplication, ApplicationSimpleDto>();

        CreateMap<OpenIddictApplication, ApplicationSecretDto>();


        CreateMap<OpenIddictToken, TokenDto>()
            .ForMember(x => x.Application, opt => opt.MapFrom<ApplicationResolver>())
            .MapExtraProperties();

        CreateMap<OpenIddictToken, TokenDetailDto>()
            .ForMember(x => x.Application, opt => opt.MapFrom<ApplicationResolver>())
            .MapExtraProperties();
    }
}
