using System;
using System.Collections.Generic;

using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace IczpNet.OpenIddict.Tokens.Dtos;

public class TokenDetailDto : TokenDto, IEntityDto<Guid>, IHasCreationTime
{

    //
    // 摘要:
    //     Gets or sets the payload of the current token, if applicable. Note: this property
    //     is only used for reference tokens and may be encrypted for security reasons.
    public virtual string Payload { get; set; }
}

