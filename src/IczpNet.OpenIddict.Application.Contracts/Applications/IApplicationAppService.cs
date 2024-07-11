using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.BaseDtos;
using System;

namespace IczpNet.OpenIddict.Applications;

public interface IApplicationAppService :ICrudAbpCommonsAppService<OpenIddictApplicationDto, OpenIddictApplicationDto, Guid, GetListInput, ApplicationCreateInput, ApplicationUpdateInput>
{
}
