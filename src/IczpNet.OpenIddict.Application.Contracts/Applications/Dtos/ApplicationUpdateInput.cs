﻿using System.Collections.Generic;
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
