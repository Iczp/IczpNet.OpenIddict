using System;
using Volo.Abp.Application.Dtos;

namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationSimpleDto : EntityDto<Guid>
{
    /// <summary>
    /// Gets or sets the client identifier associated with the application.
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Gets or sets the consent type associated with the application.
    /// </summary>
    public string ConsentType { get; set; }

    /// <summary>
    /// Gets or sets the display name associated with the application.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the application type associated with the application.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string ClientUri { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string LogoUri { get; set; }
}

