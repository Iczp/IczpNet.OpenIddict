using System;

using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;
using System.Collections.Generic;

namespace IczpNet.OpenIddict.Authorizations.Dtos;

public class AuthorizationDto : ExtensibleObject, IEntityDto<Guid>, IHasCreationTime
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the application associated with the current authorization.
    /// </summary>
    public virtual Guid? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets the UTC creation date of the current authorization.
    /// </summary>
    public virtual DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <c>null</c> if no bag was associated with the current authorization.
    /// </summary>
    public virtual string Properties { get; set; }

    /// <summary>
    /// Gets or sets the scopes associated with the current
    /// authorization, serialized as a JSON array.
    /// </summary>
    public virtual List<string> Scopes { get; set; }

    /// <summary>
    /// Gets or sets the status of the current authorization.
    /// </summary>
    public virtual string Status { get; set; }

    /// <summary>
    /// Gets or sets the subject associated with the current authorization.
    /// </summary>
    public virtual string Subject { get; set; }

    /// <summary>
    /// Gets or sets the type of the current authorization.
    /// </summary>
    public virtual string Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime CreationTime { get; set; }

}

