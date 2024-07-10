using IczpNet.OpenIddict.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;

namespace IczpNet.OpenIddict.BaseAppServices;
[ApiExplorerSettings(GroupName = OpenIddictRemoteServiceConsts.ModuleName)]
public abstract class OpenIddictAppService : ApplicationService
{
    protected OpenIddictAppService()
    {
        LocalizationResource = typeof(OpenIddictResource);
        ObjectMapperContext = typeof(OpenIddictApplicationModule);
    }
}
