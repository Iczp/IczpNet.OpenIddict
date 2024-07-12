using Volo.Abp.ObjectMapping;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.DependencyInjection;
using IczpNet.OpenIddict.Authorizations.Dtos;

namespace IczpNet.OpenIddict.Authorizations;


public class AuthorizationToDtoMapper<T> : IObjectMapper<OpenIddictAuthorization, T>, ITransientDependency where T : AuthorizationDto, new()
{
    public virtual T Map(OpenIddictAuthorization source)
    {
        if (source == null)
        {
            return null;
        }

        return new T
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            CreationDate = source.CreationDate,
            Scopes = Helper.ParseToList(source.Scopes),
            Status = source.Status,
            Subject = source.Subject,
            Type = source.Type,
            Properties = source.Properties,
            CreationTime = source.CreationTime,
        };
    }

    public virtual T Map(OpenIddictAuthorization source, T destination)
    {
        if (source == null || destination == null)
        {
            return null;
        }

        destination.Id = source.Id;
        destination.ApplicationId = source.ApplicationId;
        destination.CreationDate = source.CreationDate;
        destination.Scopes = Helper.ParseToList(source.Scopes);
        destination.Status = source.Status;
        destination.Subject = source.Subject;
        destination.Type = source.Type;
        destination.Properties = source.Properties;
        destination.CreationTime = source.CreationTime;

        return destination;
    }
}
