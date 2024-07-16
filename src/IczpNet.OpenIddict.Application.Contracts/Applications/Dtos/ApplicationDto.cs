using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.ObjectExtending;

namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationDto : ExtensibleObject, IEntityDto<Guid>, IHasCreationTime
{

    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the client identifier associated with the application.
    /// </summary>
    public string ClientId { get; set; }

    ///// <summary>
    ///// Gets or sets the client secret associated with the application.
    ///// Note: depending on the application manager used when creating it,
    ///// this property may be hashed or encrypted for security reasons.
    ///// </summary>
    //public string ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the consent type associated with the application.
    /// </summary>
    public string ConsentType { get; set; }

    /// <summary>
    /// Gets or sets the display name associated with the application.
    /// </summary>
    public string DisplayName { get; set; }

    ///// <summary>
    ///// Gets the localized display names associated with the application.
    ///// </summary>
    //public Dictionary<CultureInfo, string> DisplayNames { get; } = new();

    ///// <summary>
    ///// Gets the permissions associated with the application.
    ///// </summary>
    //public HashSet<string> Permissions { get; } = new(StringComparer.Ordinal);

    ///// <summary>
    ///// Gets the post-logout redirect URIs associated with the application.
    ///// </summary>
    //public HashSet<Uri> PostLogoutRedirectUris { get; } = new();

    ///// <summary>
    ///// Gets the additional properties associated with the application.
    ///// </summary>
    //public Dictionary<string, JsonElement> Properties { get; } = new(StringComparer.Ordinal);

    ///// <summary>
    ///// Gets the redirect URIs associated with the application.
    ///// </summary>
    //public HashSet<Uri> RedirectUris { get; } = new();

    ///// <summary>
    ///// Gets the requirements associated with the application.
    ///// </summary>
    //public HashSet<string> Requirements { get; } = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the application type associated with the application.
    /// </summary>
    public string ClientType { get; set; }

    /// <summary>
    /// Gets or sets the localized display names
    /// associated with the current application,
    /// serialized as a JSON object.
    /// </summary>
    public virtual object DisplayNames { get; set; }

    /// <summary>
    /// Gets or sets the permissions associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    public virtual List<string> Permissions { get; set; }

    /// <summary>
    /// Gets or sets the logout callback URLs associated with
    /// the current application, serialized as a JSON array.
    /// </summary>
    public virtual List<string> PostLogoutRedirectUris { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <c>null</c> if no bag was associated with the current application.
    /// </summary>
    public virtual object Properties { get; set; }

    /// <summary>
    /// Gets or sets the callback URLs associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    public virtual List<string> RedirectUris { get; set; }

    /// <summary>
    /// Gets or sets the requirements associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    public virtual List<string> Requirements { get; set; }

    /// <summary>
    /// Scopes
    /// </summary>
    public virtual List<string> Scopes { get; set; }

    /// <summary>
    /// GrantTypes
    /// </summary>
    public virtual List<string> GrantTypes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string ClientUri { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string LogoUri { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime CreationTime { get; set; }

}

