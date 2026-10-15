using System;
using Volo.Abp.ObjectMapping;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.AbpCommons.Utils;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationToDtoMapper<T> : IObjectMapper<OpenIddictApplication, T>, ITransientDependency where T : ApplicationDto, new()
{
    public virtual T Map(OpenIddictApplication source)
    {
        if (source == null)
        {
            return null;
        }

        var permissions = JsonHelper.ParseToList(source.Permissions) ?? [];

        var result = new T()
        {
            Id = source.Id,
            ClientId = source.ClientId,
            //ClientSecret = source.ClientSecret,
            DisplayName = source.DisplayName,
            ClientType = source.ClientType,
            ApplicationType = source.ApplicationType,
            Permissions = permissions,
            GrantTypes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.GrantType),
            Scopes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.Scope),
            RedirectUris = JsonHelper.ParseToList(source.RedirectUris),
            PostLogoutRedirectUris = JsonHelper.ParseToList(source.PostLogoutRedirectUris),
            ConsentType = source.ConsentType,
            DisplayNames = source.DisplayNames,
            Properties = source.Properties,
            Requirements = JsonHelper.ParseToList(source.Requirements),
            JsonWebKeySet = source.JsonWebKeySet,
            Settings = source.Settings,
            ClientUri = source.ClientUri,
            LogoUri = source.LogoUri,
            CreationTime = source.CreationTime,
        };

        MapSettings(source.Settings, result);

        return result;
    }

    private static List<string> GetPart(List<string> permissions, string prefixe)
    {
        return permissions
            .Where(x => x.StartsWith(prefixe))
            .Select(x => x[prefixe.Length..])
            .ToList();
    }


    public virtual T Map(OpenIddictApplication source, T destination)
    {
        if (source == null || destination == null)
        {
            return null;
        }
        var permissions = JsonHelper.ParseToList(source.Permissions) ?? [];
        destination.Id = source.Id;
        destination.ClientId = source.ClientId;
        //destination.ClientSecret = source.ClientSecret;
        destination.ConsentType = source.ConsentType;
        destination.ClientType = source.ClientType;
        destination.ApplicationType = source.ApplicationType;
        destination.DisplayName = source.DisplayName;
        destination.DisplayNames = source.DisplayNames;
        destination.Permissions = permissions;
        destination.Properties = source.Properties;
        destination.GrantTypes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.GrantType);
        destination.Scopes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.Scope);
        destination.RedirectUris = JsonHelper.ParseToList(source.RedirectUris);
        destination.PostLogoutRedirectUris = JsonHelper.ParseToList(source.PostLogoutRedirectUris);
        destination.Requirements = JsonHelper.ParseToList(source.Requirements);
        destination.JsonWebKeySet = source.JsonWebKeySet;
        destination.Settings = source.Settings;
        destination.ClientUri = source.ClientUri;
        destination.LogoUri = source.LogoUri;
        destination.CreationTime = source.CreationTime;

        MapSettings(source.Settings, destination);

        return destination;
    }

    private static void MapSettings(string settingsJson, ApplicationDto dto)
    {
        if (string.IsNullOrWhiteSpace(settingsJson))
        {
            return;
        }

        try
        {
            var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(settingsJson);
            if (dict != null)
            {
                if (dict.TryGetValue(OpenIddictConstants.Settings.TokenLifetimes.AccessToken, out var act) && TimeSpan.TryParse(act, out var actTs))
                {
                    dto.AccessTokenLifetime = (int)actTs.TotalSeconds;
                }

                if (dict.TryGetValue(OpenIddictConstants.Settings.TokenLifetimes.IdentityToken, out var idt) && TimeSpan.TryParse(idt, out var idtTs))
                {
                    dto.IdentityTokenLifetime = (int)idtTs.TotalSeconds;
                }

                if (dict.TryGetValue(OpenIddictConstants.Settings.TokenLifetimes.RefreshToken, out var reft) && TimeSpan.TryParse(reft, out var reftTs))
                {
                    dto.RefreshTokenLifetime = (int)reftTs.TotalSeconds;
                }

                if (dict.TryGetValue(OpenIddictConstants.Settings.TokenLifetimes.AuthorizationCode, out var auc) && TimeSpan.TryParse(auc, out var aucTs))
                {
                    dto.AuthorizationCodeLifetime = (int)aucTs.TotalSeconds;
                }

                if (dict.TryGetValue(OpenIddictConstants.Settings.TokenLifetimes.DeviceCode, out var dvc) && TimeSpan.TryParse(dvc, out var dvcTs))
                {
                    dto.DeviceCodeLifetime = (int)dvcTs.TotalSeconds;
                }

                if (dict.TryGetValue("AllowedCorsOrigins", out var cors) && !string.IsNullOrWhiteSpace(cors))
                {
                    dto.AllowedCorsOrigins = cors.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
                }
            }
        }
        catch
        {
            // Ignore settings deserialization failure
        }
    }
}
