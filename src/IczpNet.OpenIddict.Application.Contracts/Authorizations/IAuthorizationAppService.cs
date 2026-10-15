using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Authorizations.Dtos;
using IczpNet.AbpCommons.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace IczpNet.OpenIddict.Authorizations;

public interface IAuthorizationAppService :ICrudAbpCommonsAppService<AuthorizationDetailDto, AuthorizationDto, Guid, AuthorizationGetListInput, CreateInput, UpdateInput>
{

    Task<PagedResultDto<KeyValueDto<string>>> GetStatusListAsync(AuthorizationStatusGetListInput input);

    Task<PagedResultDto<KeyValueDto<Guid?>>> GetApplicationIdListAsync(AuthorizationApplicationIdGetListInput input);

    /// <summary>
    /// 主动吊销授权（并级联吊销关联的令牌）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RevokeAsync(Guid id);

    /// <summary>
    /// 批量吊销授权
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task RevokeManyAsync(System.Collections.Generic.List<Guid> ids);
}
