using System.Collections.Generic;
using System.ComponentModel;

namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationCreateInput
{
    public string ClientId { get; set; }
    public string DisplayName { get; set; }

    [DefaultValue(null)]
    public string ClientSecret { get; set; }
    public List<string> GrantTypes { get; set; }
    public List<string> Scopes { get; set; }
    public string RedirectUri { get; set; }
    public string PostLogoutRedirectUri { get; set; }

    /// <summary>
    /// supported: confidential | public
    /// </summary>
    [DefaultValue("public")]
    public string Type { get; set; }
    public string ConsentType { get; set; }
    public List<string> Permissions { get; set; }
}
