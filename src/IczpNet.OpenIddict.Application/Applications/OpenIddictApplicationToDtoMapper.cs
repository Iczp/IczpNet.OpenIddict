using Volo.Abp.ObjectMapping;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.DependencyInjection;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Linq;
using IczpNet.OpenIddict.Applications.Dtos;

namespace IczpNet.OpenIddict.Applications;


public class OpenIddictApplicationToDtoMapper : IObjectMapper<OpenIddictApplication, OpenIddictApplicationDto>, ITransientDependency
{

    private static List<string> ParseToList(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }
        using var document = JsonDocument.Parse(json);

        var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

        foreach (var element in document.RootElement.EnumerateArray())
        {
            var value = element.GetString();
            if (string.IsNullOrEmpty(value))
            {
                continue;
            }

            builder.Add(value);
        }

        return [.. builder];
    }

    public OpenIddictApplicationDto Map(OpenIddictApplication source)
    {
        if (source == null)
        {
            return null;
        }

        var permissions = ParseToList(source.Permissions) ?? [];

        return new OpenIddictApplicationDto
        {
            Id = source.Id,
            ClientId = source.ClientId,
            DisplayName = source.DisplayName,
            Type = source.Type,
            ClientSecret = source.ClientSecret,
            Permissions = permissions,
            GrantTypes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.GrantType),
            Scopes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.Scope),
            RedirectUris = ParseToList(source.RedirectUris),
            PostLogoutRedirectUris = ParseToList(source.PostLogoutRedirectUris),
            ConsentType = source.ConsentType,
            DisplayNames = source.DisplayNames,
            Properties = source.Properties,
            Requirements = ParseToList(source.Requirements),
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


    public OpenIddictApplicationDto Map(OpenIddictApplication source, OpenIddictApplicationDto destination)
    {
        if (source == null || destination == null)
        {
            return null;
        }
        var permissions = ParseToList(source.Permissions) ?? [];
        destination.Id = source.Id;
        destination.ClientId = source.ClientId;
        destination.ClientSecret = source.ClientSecret;
        destination.ConsentType = source.ConsentType;
        destination.DisplayName = source.DisplayName;
        destination.DisplayNames = source.DisplayNames;
        destination.Permissions = permissions;
        destination.Properties = source.Properties;
        destination.GrantTypes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.GrantType);
        destination.Scopes = GetPart(permissions, OpenIddictConstants.Permissions.Prefixes.Scope);
        destination.RedirectUris = ParseToList(source.RedirectUris);
        destination.PostLogoutRedirectUris = ParseToList(source.PostLogoutRedirectUris);
        destination.Requirements = ParseToList(source.Requirements);
        destination.Type = source.Type;
        destination.ClientUri = source.ClientUri;
        destination.LogoUri = source.LogoUri;
        destination.CreationTime = source.CreationTime;

        return destination;
    }
}
