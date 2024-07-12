using Volo.Abp.ObjectMapping;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using IczpNet.OpenIddict.Scopes.Dtos;

namespace IczpNet.OpenIddict.Scopes;


public class ScopeToDtoMapper<T> : IObjectMapper<OpenIddictScope, T>, ITransientDependency where T : ScopeDto, new()
{
    public virtual T Map(OpenIddictScope source)
    {
        if (source == null)
        {
            return null;
        }

        var resources = Helper.ParseToList(source.Resources) ?? [];

        return new T
        {
            Id = source.Id,
            Name = source.Name,
            DisplayName = source.DisplayName,
            Description = source.Description,
            Descriptions = source.Descriptions,
            DisplayNames = source.DisplayNames,
            Resources = resources,
            Properties = source.Properties,
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


    public virtual T Map(OpenIddictScope source, T destination)
    {
        if (source == null || destination == null)
        {
            return null;
        }
        var permissions = Helper.ParseToList(source.Resources) ?? [];
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.DisplayName = source.DisplayName;
        destination.Description = source.Description;
        destination.Descriptions = source.Descriptions;
        destination.DisplayNames = source.DisplayNames;
        destination.Resources = permissions;
        destination.Properties = source.Properties;
        destination.CreationTime = source.CreationTime;

        return destination;
    }
}
