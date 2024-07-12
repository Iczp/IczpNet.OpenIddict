using IczpNet.OpenIddict.BaseDtos;
using System;
using System.ComponentModel;

namespace IczpNet.OpenIddict.Tokens.Dtos;

public class TokenGetListInput : GetListInput
{

    //
    // 摘要:
    //     Gets or sets the application associated with the current token.
    public virtual Guid? ApplicationId { get; set; }

    //
    // 摘要:
    //     Gets or sets the authorization associated with the current token.
    public virtual Guid? AuthorizationId { get; set; }


    

    //
    // 摘要:
    //     Gets or sets the reference identifier associated with the current token, if applicable.
    //     Note: this property is only used for reference tokens and may be hashed or encrypted
    //     for security reasons.
    public virtual string ReferenceId { get; set; }

    //
    // 摘要:
    //     Gets or sets the status of the current token.
    public virtual string Status { get; set; }

    //
    // 摘要:
    //     Gets or sets the subject associated with the current token.
    public virtual string Subject { get; set; }

    //
    // 摘要:
    //     Gets or sets the type of the current token.
    public virtual string Type { get; set; }

    /// <summary>
    /// Start CreationDate
    /// </summary>
    public virtual DateTime? StartCreationDate { get; set; }

    /// <summary>
    /// End CreationDate
    /// </summary>
    public virtual DateTime? EndCreationDate { get; set; }

    /// <summary>
    /// public virtual DateTime? ExpirationDate { get; set; }
    /// </summary>
    public virtual DateTime? StartExpirationDate { get; set; }

    /// <summary>
    /// EndExpirationDate
    /// </summary>
    public virtual DateTime? EndExpirationDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DateTime? StartRedemptionDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DateTime? EndRedemptionDate { get; set; }

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
