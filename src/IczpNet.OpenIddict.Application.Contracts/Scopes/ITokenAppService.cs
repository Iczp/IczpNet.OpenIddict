using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Tokens.Dtos;
using System;

namespace IczpNet.OpenIddict.Tokens;

public interface ITokenAppService :ICrudAbpCommonsAppService<TokenDetailDto, TokenDto, Guid, TokenGetListInput, TokenCreateInput, TokenUpdateInput>
{

}
