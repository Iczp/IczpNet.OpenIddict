using System;
using System.Collections.Generic;

using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;
using IczpNet.OpenIddict.Applications.Dtos;

namespace IczpNet.OpenIddict.Tokens.Dtos;

public class TokenDto : ExtensibleObject, IEntityDto<Guid>, IHasCreationTime
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    //
    // 摘要:
    //     Gets or sets the application associated with the current token.
    public virtual Guid? ApplicationId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual ApplicationSimpleDto Application { get; set; }

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
    public DateTime CreationTime { get; set; }

}

