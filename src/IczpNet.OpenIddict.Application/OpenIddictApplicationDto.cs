using System;
using System.Collections.Generic;
using Volo.Abp.OpenIddict.Applications;

namespace Rctea.IM;

public class OpenIddictApplicationDto : AbpApplicationDescriptor
{

    public Guid Id { get; set; }


    /// <summary>
    /// Gets or sets the localized display names
    /// associated with the current application,
    /// serialized as a JSON object.
    /// </summary>
    public new virtual object DisplayNames { get; set; }

    /// <summary>
    /// Gets or sets the permissions associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    public new virtual List<string> Permissions { get; set; }

    /// <summary>
    /// Gets or sets the logout callback URLs associated with
    /// the current application, serialized as a JSON array.
    /// </summary>
    public new virtual List<string> PostLogoutRedirectUris { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <c>null</c> if no bag was associated with the current application.
    /// </summary>
    public new virtual object Properties { get; set; }

    /// <summary>
    /// Gets or sets the callback URLs associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    public new virtual List<string> RedirectUris { get; set; }

    /// <summary>
    /// Gets or sets the requirements associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    public new virtual List<string> Requirements { get; set; }

    /// <summary>
    /// Scopes
    /// </summary>
    public virtual List<string> Scopes { get; set; }

    /// <summary>
    /// GrantTypes
    /// </summary>
    public virtual List<string> GrantTypes { get; set; }
}

