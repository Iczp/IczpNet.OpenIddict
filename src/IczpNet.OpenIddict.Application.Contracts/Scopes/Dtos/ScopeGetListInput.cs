using IczpNet.OpenIddict.BaseDtos;
using System;
using System.ComponentModel;

namespace IczpNet.OpenIddict.Scopes.Dtos;

public class ScopeGetListInput : GetListInput
{

    /// <summary>
    /// Gets or sets the unique name associated with the current scope.
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the resources associated with the
    /// current scope, serialized as a JSON array.
    /// </summary>
    public virtual string Resources { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DateTime? StartCreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DateTime? EndCreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DateTime? StartLastModificationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DateTime? EndLastModificationTime { get; set; }
}
