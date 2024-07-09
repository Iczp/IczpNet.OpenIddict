using IczpNet.OpenIddict.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace IczpNet.OpenIddict;

public abstract class OpenIddictController : AbpControllerBase
{
    protected OpenIddictController()
    {
        LocalizationResource = typeof(OpenIddictResource);
    }
}
