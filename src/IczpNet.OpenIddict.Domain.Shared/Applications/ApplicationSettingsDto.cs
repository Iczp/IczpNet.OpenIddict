using System;
using System.Collections.Generic;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationSettingsDto
{
    /// <summary>
    /// 访问令牌生命周期 (秒)
    /// </summary>
    public virtual int? AccessTokenLifetime { get; set; }

    /// <summary>
    /// 身份令牌生命周期 (秒)
    /// </summary>
    public virtual int? IdentityTokenLifetime { get; set; }

    /// <summary>
    /// 刷新令牌生命周期 (秒)
    /// </summary>
    public virtual int? RefreshTokenLifetime { get; set; }

    /// <summary>
    /// 授权码有效期 (秒)
    /// </summary>
    public virtual int? AuthorizationCodeLifetime { get; set; }

    /// <summary>
    /// 设备码有效期 (秒)
    /// </summary>
    public virtual int? DeviceCodeLifetime { get; set; }

    /// <summary>
    /// 允许的跨域来源列表 (CORS Origins)
    /// </summary>
    public virtual List<string> AllowedCorsOrigins { get; set; } = new();
}
