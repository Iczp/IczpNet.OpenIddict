using IczpNet.AbpCommons;
using IczpNet.AbpCommons.Dtos;
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

    /// <summary>
    /// 主动吊销单个令牌
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RevokeAsync(Guid id);

    /// <summary>
    /// 按用户吊销其所有令牌（强制下线）
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    Task RevokeBySubjectAsync(string subject);

    /// <summary>
    /// 批量吊销令牌
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task RevokeManyAsync(System.Collections.Generic.List<Guid> ids);
}
