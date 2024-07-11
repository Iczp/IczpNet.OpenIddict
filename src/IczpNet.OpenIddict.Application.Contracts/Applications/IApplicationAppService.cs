using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.BaseDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IczpNet.OpenIddict.Applications;

public interface IApplicationAppService :ICrudAbpCommonsAppService<OpenIddictApplicationDto, OpenIddictApplicationDto, Guid, ApplicationGetListInput, ApplicationCreateInput, ApplicationUpdateInput>
{
    Task<OpenIddictApplicationDto> GetByClientIdAsync(string cliendId);

    Task DeleteByClientIdAsync(string cliendId);

    Task DeleteManyByClientIdAsync(List<string> cliendIds);
}
