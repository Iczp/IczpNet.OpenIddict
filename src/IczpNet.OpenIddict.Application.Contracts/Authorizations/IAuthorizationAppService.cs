using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Authorizations.Dtos;
using IczpNet.OpenIddict.BaseDtos;
using System;

namespace IczpNet.OpenIddict.Authorizations;

public interface IAuthorizationAppService :ICrudAbpCommonsAppService<AuthorizationDetailDto, AuthorizationDto, Guid, AuthorizationGetListInput, CreateInput, UpdateInput>
{

}
