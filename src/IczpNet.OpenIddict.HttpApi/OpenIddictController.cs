using IczpNet.OpenIddict.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace IczpNet.OpenIddict;

[Area(OpenIddictRemoteServiceConsts.ModuleName)]
[RemoteService(Name = OpenIddictRemoteServiceConsts.RemoteServiceName)]
public abstract class OpenIddictController : AbpControllerBase
{
    protected OpenIddictController()
    {
        LocalizationResource = typeof(OpenIddictResource);
    }
}
