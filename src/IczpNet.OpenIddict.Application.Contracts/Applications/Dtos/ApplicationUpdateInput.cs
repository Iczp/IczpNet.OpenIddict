using System.Collections.Generic;
using System.ComponentModel;

namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationUpdateInput
{
    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(null)]
    public string ClientId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(null)] 
    public string DisplayName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(null)] 
    public string ClientSecret { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<string> GrantTypes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<string> Scopes { get; set; }

    public string RedirectUri { get; set; }

    public string PostLogoutRedirectUri { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ConsentType { get; set; }

    /// <summary>
    /// URI to further information about client.
    /// </summary>
    [DefaultValue(null)]
    public string ClientUri { get; set; }

    /// <summary>
    /// URI to client logo.
    /// </summary>
    [DefaultValue(null)] 
    public string LogoUri { get; set; }
}
