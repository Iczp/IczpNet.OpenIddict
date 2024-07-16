using IczpNet.AbpCommons.Utils;
using IczpNet.OpenIddict.Localization;
using IczpNet.OpenIddict.Permissions;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Reflection;


namespace IczpNet.OpenIddict.constants;
[ApiExplorerSettings(GroupName = OpenIddictRemoteServiceConsts.ModuleName)]
public class ConstsAppService : ApplicationService
{
    protected ConstsAppService()
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

    public virtual async Task<Dictionary<string, string>> GetClientTypesAsync()
    {
        return await Task.FromResult(GetFields(typeof(OpenIddictConstants.ClientTypes)));
    }

    public virtual async Task<Dictionary<string, string>> GetGrantTypesAsync()
    {
        return await Task.FromResult(GetFields(typeof(OpenIddictConstants.GrantTypes)));
    }

    public virtual async Task<Dictionary<string, string>> GetConsentTypesAsync()
    {
        return await Task.FromResult(GetFields(typeof(OpenIddictConstants.ConsentTypes)));
    }

    public virtual Task<Dictionary<string, string>> GetScopesAsync()
    {
        return Task.FromResult(GetFields(typeof(OpenIddictConstants.Scopes)));
    }

    private static Dictionary<string, string> GetFields(Type type)
    {
        var result = new Dictionary<string, string>();
        var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        foreach (FieldInfo fieldInfo in fields)
        {
            if (fieldInfo.IsLiteral && !fieldInfo.IsInitOnly && fieldInfo.FieldType == typeof(string))
            {
                string key = fieldInfo.Name;
                result[key] = (string)fieldInfo.GetValue(null);
            }
        }
        return result;
    }
}
