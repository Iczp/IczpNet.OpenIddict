using IczpNet.OpenIddict.Applications.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace IczpNet.OpenIddict.Applications;

public interface IApplicationDtoResolver : ITransientDependency
{
    ApplicationSimpleDto GetDto(Guid? appId);

    Task<ApplicationSimpleDto> GetDtoAsync(Guid? appId);
}
