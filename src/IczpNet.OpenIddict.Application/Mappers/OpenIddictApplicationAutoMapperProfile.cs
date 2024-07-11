using AutoMapper;
using IczpNet.OpenIddict.Tokens.Dtos;
using Volo.Abp.OpenIddict.Tokens;

namespace IczpNet.OpenIddict.Mappers;

public class OpenIddictApplicationAutoMapperProfile : Profile
{
    public OpenIddictApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<OpenIddictToken, TokenDto>().MapExtraProperties();
        CreateMap<OpenIddictToken, TokenDetailDto>().MapExtraProperties();
    }
}
