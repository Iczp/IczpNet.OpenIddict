using Volo.Abp.ObjectMapping;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using IczpNet.OpenIddict.Applications.Dtos;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationToDtoMapper<T> : IObjectMapper<OpenIddictApplication, T>, ITransientDependency where T : ApplicationDto, new()
{
    public virtual T Map(OpenIddictApplication source)
    {
        if (source == null)
        {
            return null;
        }

        var permissions = Helper.ParseToList(source.Permissions) ?? [];

        return new T()
        {
            Id = source.Id,
            ClientId = source.ClientId,
            DisplayName = source.DisplayName,
            Type = source.Type,
            //ClientSecret = source.ClientSecret,
            Permissions = permissions,
            GrantTypes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.GrantType),
            Scopes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.Scope),
            RedirectUris = Helper.ParseToList(source.RedirectUris),
            PostLogoutRedirectUris = Helper.ParseToList(source.PostLogoutRedirectUris),
            ConsentType = source.ConsentType,
            DisplayNames = source.DisplayNames,
            Properties = source.Properties,
            Requirements = Helper.ParseToList(source.Requirements),
            ClientUri = source.ClientUri,
            LogoUri = source.LogoUri,
            CreationTime = source.CreationTime,
        };
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
        var permissions = Helper.ParseToList(source.Permissions) ?? [];
        destination.Id = source.Id;
        destination.ClientId = source.ClientId;
        //destination.ClientSecret = source.ClientSecret;
        destination.ConsentType = source.ConsentType;
        destination.DisplayName = source.DisplayName;
        destination.DisplayNames = source.DisplayNames;
        destination.Permissions = permissions;
        destination.Properties = source.Properties;
        destination.GrantTypes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.GrantType);
        destination.Scopes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.Scope);
        destination.RedirectUris = Helper.ParseToList(source.RedirectUris);
        destination.PostLogoutRedirectUris = Helper.ParseToList(source.PostLogoutRedirectUris);
        destination.Requirements = Helper.ParseToList(source.Requirements);
        destination.Type = source.Type;
        destination.ClientUri = source.ClientUri;
        destination.LogoUri = source.LogoUri;
        destination.CreationTime = source.CreationTime;

        return destination;
    }
}
