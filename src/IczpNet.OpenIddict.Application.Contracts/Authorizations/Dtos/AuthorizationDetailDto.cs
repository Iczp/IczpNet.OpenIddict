using System;

using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;

namespace IczpNet.OpenIddict.Authorizations.Dtos;

public class AuthorizationDetailDto : AuthorizationDto, IEntityDto<Guid>, IHasCreationTime
{

}

