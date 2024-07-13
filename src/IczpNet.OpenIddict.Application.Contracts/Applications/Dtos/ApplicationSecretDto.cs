using System;
using Volo.Abp.Application.Dtos;

namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationSecretDto : ApplicationSecretInput, IEntityDto<Guid>
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }
}

