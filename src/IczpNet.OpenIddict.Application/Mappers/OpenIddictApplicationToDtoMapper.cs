using Volo.Abp.ObjectMapping;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.DependencyInjection;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Linq;
using IczpNet.OpenIddict.Applications.Dtos;

namespace IczpNet.OpenIddict.Mappers;


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
            GrantTypes = permissions.Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType)).ToList(),
            Scopes = permissions.Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope)).ToList(),
            RedirectUris = ParseToList(source.RedirectUris),
            PostLogoutRedirectUris = ParseToList(source.PostLogoutRedirectUris),
            ConsentType = source.ConsentType,
            DisplayNames = source.DisplayNames,
            Properties = source.Properties,
            Requirements = ParseToList(source.Requirements),
            ClientUri = source.ClientUri,
            LogoUri = source.LogoUri
        };
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
        destination.GrantTypes = permissions.Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType)).ToList();
        destination.Scopes = permissions.Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope)).ToList();
        destination.RedirectUris = ParseToList(source.RedirectUris);
        destination.PostLogoutRedirectUris = ParseToList(source.PostLogoutRedirectUris);
        destination.Requirements = ParseToList(source.Requirements);
        destination.Type = source.Type;
        destination.ClientUri = source.ClientUri;
        destination.LogoUri = source.LogoUri;

        return destination;
    }
}
