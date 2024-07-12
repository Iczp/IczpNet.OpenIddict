using IczpNet.OpenIddict.Localization;
using IczpNet.OpenIddict.Permissions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Reflection;

namespace IczpNet.OpenIddict.BaseAppServices;
[ApiExplorerSettings(GroupName = OpenIddictRemoteServiceConsts.ModuleName)]
public class CultureAppServiceAppService : ApplicationService
{
    protected CultureAppServiceAppService()
    {
        LocalizationResource = typeof(OpenIddictResource);
        ObjectMapperContext = typeof(OpenIddictApplicationModule);
    }

    public virtual Task<Dictionary<string, string>> GetCultureAsync(string keyTemp = "Permission:{{key}}", string valueTemp = "{{key}}")
    {
        var dict = new Dictionary<string, string>();

        var rootPermissionType = typeof(OpenIddictPermissions);

        foreach (var nestedType in rootPermissionType.GetNestedTypes())
        {
            var names = ReflectionHelper.GetPublicConstantsRecursively(nestedType);

            foreach (var name in names)
            {
                var key = keyTemp.Replace("{{key}}", name);

                var value = valueTemp.Replace("{{key}}", name);

                dict.TryAdd(key, value);
            }
        }
        return Task.FromResult(dict);
    }
}
