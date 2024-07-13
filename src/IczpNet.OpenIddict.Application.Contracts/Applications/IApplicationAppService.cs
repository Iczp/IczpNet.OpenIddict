using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.Authorizations.Dtos;
using IczpNet.AbpCommons.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace IczpNet.OpenIddict.Applications;

public interface IApplicationAppService :ICrudAbpCommonsAppService<ApplicationDetailDto, ApplicationDto, Guid, ApplicationGetListInput, ApplicationCreateInput, ApplicationUpdateInput>
{
    Task<ApplicationDto> GetByClientIdAsync(string clientId);

    Task DeleteByClientIdAsync(string clientId);

    Task DeleteManyByClientIdAsync(List<string> cliendIds);

    Task<PagedResultDto<KeyValueDto<string>>> GetTypeListAsync(ApplicationTypeGetListInput input);

    Task<PagedResultDto<KeyValueDto<string>>> GetConsentTypeListAsync(ApplicationConsentTypeGetListInput input);

    Task<ApplicationSecretDto> GetSecretByClientIdAsync(string clientId);

    Task<ApplicationSecretDto> GetClientSecretAsync(Guid id);

    Task<ApplicationSecretDto> SetClientSecretAsync(ApplicationSecretInput input);

}
