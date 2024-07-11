using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IczpNet.OpenIddict.Scopes.Dtos;
using IczpNet.OpenIddict.BaseAppServices;
using IczpNet.OpenIddict.Permissions;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Scopes;
using IczpNet.AbpCommons;
using IczpNet.AbpCommons.Extensions;

namespace IczpNet.OpenIddict.Scopes;

public class ScopeAppService : CrudOpenIddictAppService<OpenIddictScope, ScopeDto, ScopeDto, Guid, ScopeGetListInput, ScopeCreateInput, ScopeUpdateInput>, IScopeAppService
{

    protected override string GetListPolicyName { get; set; } = OpenIddictPermissions.ScopePermissions.GetList;
    protected override string GetPolicyName { get; set; } = OpenIddictPermissions.ScopePermissions.GetItem;
    protected override string CreatePolicyName { get; set; } = OpenIddictPermissions.ScopePermissions.Create;
    protected override string UpdatePolicyName { get; set; } = OpenIddictPermissions.ScopePermissions.Update;
    protected override string DeletePolicyName { get; set; } = OpenIddictPermissions.ScopePermissions.Delete;
    protected IScopeManager ScopeManager { get; set; }

    public ScopeAppService(
        IRepository<OpenIddictScope, Guid> repository,
        IScopeManager scopeManager) : base(repository)
    {
        ScopeManager = scopeManager;
    }


    protected override async Task<IQueryable<OpenIddictScope>> CreateFilteredQueryAsync(ScopeGetListInput input)
    {
        var query = (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.Name.IsNullOrEmpty(), x => x.Name.Equals(input.Name))
            .WhereIf(!input.Resources.IsNullOrEmpty(), x => x.Resources.Contains(input.Resources))
            .WhereIf(input.StartLastModificationTime.HasValue, x => x.LastModificationTime >= input.StartLastModificationTime)
            .WhereIf(input.EndLastModificationTime.HasValue, x => x.LastModificationTime < input.EndLastModificationTime)
            .WhereIf(input.StartCreationTime.HasValue, x => x.CreationTime >= input.StartCreationTime)
            .WhereIf(input.EndCreationTime.HasValue, x => x.CreationTime < input.EndCreationTime)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword) || x.DisplayName.Contains(input.Keyword))
            ;

        return query;
    }

    [HttpPost]
    public override async Task<ScopeDto> CreateAsync(ScopeCreateInput input)
    {
        await CheckCreatePolicyAsync(input);

        await CheckCreateAsync(input);

        var application = await ScopeManager.CreateAsync(
             name: input.Name,
             displayName: input.DisplayName,
             resources: input.Resources);

        var entity = await GetEntityByIdAsync(application.Id);

        return await MapToGetOutputDtoAsync(entity);
    }

    [HttpPost]
    public override async Task<ScopeDto> UpdateAsync(Guid id, ScopeUpdateInput input)
    {
        return await base.UpdateAsync(id, input);
    }

    [HttpPost]
    public async Task<ScopeDto> GetByNameAsync(string name)
    {
        await CheckGetPolicyAsync();

        var app = Assert.NotNull(await FindByNameAsync(name), $"No such scope,name:{name}");

        return await GetAsync(app.Id);
    }

    protected virtual async Task<OpenIddictScopeModel> FindByNameAsync(string name)
    {
        return (await ScopeManager.FindByNameAsync(name)).As<OpenIddictScopeModel>();
    }

    [HttpPost]
    public async Task DeleteByNameAsync(string name)
    {
        var app = Assert.NotNull(await FindByNameAsync(name), $"No such scope,name:{name}");

        await DeleteAsync(app.Id);
    }

    protected override async Task DeleteByIdAsync(Guid id)
    {
        await ScopeManager.DeleteAsync(await ScopeManager.FindByIdAsync(id.ToString()));
    }

    [HttpPost]
    public async Task DeleteManyByNameAsync(List<string> names)
    {
        Assert.If(!names.IsAny(), "names is null");

        var idList = new List<Guid>();

        foreach (var name in names)
        {
            var app = Assert.NotNull(await FindByNameAsync(name), $"No such scope,name:{name}");
            idList.Add(app.Id);
        }

        await DeleteManyAsync(idList);
    }
}
