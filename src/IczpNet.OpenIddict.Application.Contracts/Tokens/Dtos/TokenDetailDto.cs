using System;
using Volo.Abp.Application.Dtos;

namespace IczpNet.OpenIddict.Tokens.Dtos;

public class TokenDetailDto : TokenDto, IEntityDto<Guid>
{

    //
    // 摘要:
    //     Gets or sets the payload of the current token, if applicable. Note: this property
    //     is only used for reference tokens and may be encrypted for security reasons.
    public virtual string Payload { get; set; }
}

