using IczpNet.OpenIddict.Localization;
using IczpNet.OpenIddict.Permissions;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Reflection;


namespace IczpNet.OpenIddict.constants;
[ApiExplorerSettings(GroupName = OpenIddictRemoteServiceConsts.ModuleName)]
public class ConstsAppServiceAppService : ApplicationService
{
    protected ConstsAppServiceAppService()
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


    public virtual async Task<Dictionary<string, object>> GetOpenIddictConstantsTreeAsync()
    {
        return await Task.FromResult(ReflectHelper.GetConstantsTreeDictionary(typeof(OpenIddictConstants)));
    }

    public virtual async Task<Dictionary<string, string>> GetOpenIddictConstantsAsync()
    {
        return await Task.FromResult(ReflectHelper.GetConstantsFlatDictionary(typeof(OpenIddictConstants)));
    }

    
}
