using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationDetailDto : ApplicationDto, IEntityDto<Guid>, IHasCreationTime
{

}

