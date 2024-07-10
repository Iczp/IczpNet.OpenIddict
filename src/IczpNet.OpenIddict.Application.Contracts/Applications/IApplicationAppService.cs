using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.BaseDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IczpNet.OpenIddict.Applications;

public interface IApplicationAppService :ICrudAbpCommonsAppService<OpenIddictApplicationDto, OpenIddictApplicationDto, Guid, GetListInput, CreateInput, UpdateInput>
{
}
