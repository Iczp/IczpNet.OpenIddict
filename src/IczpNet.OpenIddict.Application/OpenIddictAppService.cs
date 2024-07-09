using IczpNet.OpenIddict.Localization;
using Volo.Abp.Application.Services;

namespace IczpNet.OpenIddict;

public abstract class OpenIddictAppService : ApplicationService
{
    protected OpenIddictAppService()
    {
        LocalizationResource = typeof(OpenIddictResource);
        ObjectMapperContext = typeof(OpenIddictApplicationModule);
    }
}
