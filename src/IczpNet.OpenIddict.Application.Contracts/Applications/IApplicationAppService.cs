using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IczpNet.OpenIddict.Applications;

public interface IApplicationAppService :ICrudAbpCommonsAppService<ApplicationDto, ApplicationDto, Guid, ApplicationGetListInput, ApplicationCreateInput, ApplicationUpdateInput>
{
    Task<ApplicationDto> GetByClientIdAsync(string clientId);

    Task DeleteByClientIdAsync(string clientId);

    Task DeleteManyByClientIdAsync(List<string> cliendIds);
}
