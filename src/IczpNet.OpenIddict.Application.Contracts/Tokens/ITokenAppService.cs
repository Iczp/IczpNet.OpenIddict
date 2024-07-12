using IczpNet.AbpCommons;
using IczpNet.OpenIddict.BaseDtos;
using IczpNet.OpenIddict.Tokens.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace IczpNet.OpenIddict.Tokens;

public interface ITokenAppService :ICrudAbpCommonsAppService<TokenDetailDto, TokenDto, Guid, TokenGetListInput, TokenCreateInput, TokenUpdateInput>
{
    /// <summary>
    /// Token Type List
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<KeyValueDto<string>>> GetTypeListAsync(TokenTypeGetListInput input);

    /// <summary>
    /// Token Status List
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<KeyValueDto<string>>> GetStatusListAsync(TokenStatusGetListInput input);
}
