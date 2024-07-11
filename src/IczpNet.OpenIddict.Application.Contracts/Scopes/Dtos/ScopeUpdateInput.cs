using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IczpNet.OpenIddict.Scopes.Dtos;

public class ScopeUpdateInput
{
    /// <summary>
    /// Gets or sets the unique name associated with the current scope.
    /// </summary>
    [Required]
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the display name associated with the current scope.
    /// </summary>
    public virtual string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the public description associated with the current scope.
    /// </summary>
    public virtual string Description { get; set; }

    /// <summary>
    /// Gets or sets the resources associated with the
    /// current scope, serialized as a JSON array.
    /// </summary>
    public virtual List<string> Resources { get; set; } = [];
}
