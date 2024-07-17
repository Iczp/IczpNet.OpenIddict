using System.Collections.Generic;
using System.ComponentModel;

namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationUpdateInput
{
    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(null)]
    public virtual string ClientId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(null)] 
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the JSON Web Key Set associated with
    /// the application, serialized as a JSON object.
    /// </summary>
    public virtual string JsonWebKeySet { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public virtual List<string> GrantTypes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual List<string> Scopes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string RedirectUri { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string PostLogoutRedirectUri { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [DefaultValue("public")]
    public virtual string ClientType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string ConsentType { get; set; }

    /// <summary>
    /// Gets or sets the application type associated with the application.
    /// </summary>
    public virtual string ApplicationType { get; set; }

    /// <summary>
    /// Gets or sets the settings serialized as a JSON object.
    /// </summary>
    public virtual string Settings { get; set; }

    /// <summary>
    /// URI to further information about client.
    /// </summary>
    [DefaultValue(null)]
    public virtual string ClientUri { get; set; }

    /// <summary>
    /// URI to client logo.
    /// </summary>
    [DefaultValue(null)]
    public virtual string LogoUri { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual List<string> Permissions { get; set; }
}
