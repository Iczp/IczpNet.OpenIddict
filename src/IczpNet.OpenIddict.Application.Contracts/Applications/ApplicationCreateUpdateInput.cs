using System;
using System.Collections.Generic;
using System.Text;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationCreateUpdateInput
{
    public string ClientId { get; set; }
    public string DisplayName { get; set; }
    public string ClientSecret { get; set; }
    public List<string> GrantTypes { get; set; }
    public List<string> Scopes { get; set; }
    public string RedirectUri { get; set; }
    public string PostLogoutRedirectUri { get; set; }
}
