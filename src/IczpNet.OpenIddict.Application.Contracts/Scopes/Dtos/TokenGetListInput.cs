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
    //     Gets or sets the UTC creation date of the current token.
    public virtual DateTime? CreationDate { get; set; }

    //
    // 摘要:
    //     Gets or sets the UTC expiration date of the current token.
    public virtual DateTime? ExpirationDate { get; set; }

    //
    // 摘要:
    //     Gets or sets the payload of the current token, if applicable. Note: this property
    //     is only used for reference tokens and may be encrypted for security reasons.
    public virtual string Payload { get; set; }

    //
    // 摘要:
    //     Gets or sets the additional properties serialized as a JSON object, or null if
    //     no bag was associated with the current token.
    public virtual string Properties { get; set; }

    //
    // 摘要:
    //     Gets or sets the UTC redemption date of the current token.
    public virtual DateTime? RedemptionDate { get; set; }

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
