using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IczpNet.OpenIddict.Applications;

public interface IApplicationAppService :ICrudAbpCommonsAppService<ApplicationDetailDto, ApplicationDto, Guid, ApplicationGetListInput, ApplicationCreateInput, ApplicationUpdateInput>
{
    Task<ApplicationDto> GetByClientIdAsync(string clientId);

    Task DeleteByClientIdAsync(string clientId);

    Task DeleteManyByClientIdAsync(List<string> cliendIds);

    Task<ApplicationSecretDto> GetSecretByClientIdAsync(string clientId);

    Task<ApplicationSecretDto> GetSecretAsync(Guid id);
}
