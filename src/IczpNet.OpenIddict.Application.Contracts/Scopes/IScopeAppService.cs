using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Scopes.Dtos;
using IczpNet.OpenIddict.BaseDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IczpNet.OpenIddict.Scopes;

public interface IScopeAppService :ICrudAbpCommonsAppService<ScopeDto, ScopeDto, Guid, ScopeGetListInput, ScopeCreateInput, ScopeUpdateInput>
{
    Task<ScopeDto> GetByNameAsync(string name);

    Task DeleteByNameAsync(string name);

    Task DeleteManyByNameAsync(List<string> names);
}
