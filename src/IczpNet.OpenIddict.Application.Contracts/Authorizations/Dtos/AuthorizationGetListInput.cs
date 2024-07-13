using IczpNet.AbpCommons.Dtos;
using System;
using System.ComponentModel;

namespace IczpNet.OpenIddict.Authorizations.Dtos;

public class AuthorizationGetListInput : GetListInput
{

    //
    // 摘要:
    //     Gets or sets the application associated with the current authorization.
    public virtual Guid? ApplicationId { get; set; }

    //
    // 摘要:
    //     Gets or sets the UTC creation date of the current authorization.
    public virtual DateTime? CreationDate { get; set; }

    //
    // 摘要:
    //     Gets or sets the status of the current authorization.
    public virtual string Status { get; set; }

    //
    // 摘要:
    //     Gets or sets the subject associated with the current authorization.
    public virtual string Subject { get; set; }

    //
    // 摘要:
    //     Gets or sets the type of the current authorization.
    public virtual string Type { get; set; }

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
