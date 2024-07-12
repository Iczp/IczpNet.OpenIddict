using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Authorizations.Dtos;
using IczpNet.OpenIddict.BaseDtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace IczpNet.OpenIddict.Authorizations;

public interface IAuthorizationAppService :ICrudAbpCommonsAppService<AuthorizationDetailDto, AuthorizationDto, Guid, AuthorizationGetListInput, CreateInput, UpdateInput>
{

    Task<PagedResultDto<KeyValueDto<string>>> GetStatusListAsync(AuthorizationStatusGetListInput input);

    Task<PagedResultDto<KeyValueDto<Guid?>>> GetApplicationIdListAsync(AuthorizationApplicationIdGetListInput input);

}
