using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IczpNet.AbpCommons;
using IczpNet.AbpCommons.Extensions;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.BaseAppServices;
using IczpNet.OpenIddict.BaseDtos;
using IczpNet.OpenIddict.Permissions;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Applications;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationAppService : CrudOpenIddictAppService<OpenIddictApplication, ApplicationDto, ApplicationDto, Guid, ApplicationGetListInput, ApplicationCreateInput, ApplicationUpdateInput>, IApplicationAppService
{

    protected override string GetListPolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.GetList;
    protected override string GetPolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.GetItem;
    protected override string CreatePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Create;
    protected override string UpdatePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Update;
    protected override string DeletePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Delete;
    protected IApplicationManager ApplicationManager { get; set; }

    public ApplicationAppService(
        IRepository<OpenIddictApplication, Guid> repository,
        IApplicationManager applicationManager) : base(repository)
    {
        ApplicationManager = applicationManager;
    }


    protected override async Task<IQueryable<OpenIddictApplication>> CreateFilteredQueryAsync(ApplicationGetListInput input)
    {
        var query = (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.ClientId.IsNullOrEmpty(), x => x.ClientId.Equals(input.ClientId))
            .WhereIf(!input.ClientType.IsNullOrEmpty(), x => x.Type.Equals(input.ClientType))
            .WhereIf(!input.ConsentType.IsNullOrEmpty(), x => x.ConsentType.Equals(input.ConsentType))
            .WhereIf(!input.ClientUri.IsNullOrEmpty(), x => x.ClientUri.StartsWith(input.ClientUri))
            .WhereIf(!input.DisplayName.IsNullOrEmpty(), x => x.DisplayName.StartsWith(input.DisplayName))
            .WhereIf(!input.LogoUri.IsNullOrEmpty(), x => x.LogoUri.StartsWith(input.LogoUri))
            .WhereIf(!input.RedirectUri.IsNullOrEmpty(), x => x.RedirectUris.Contains(input.RedirectUri))
            .WhereIf(!input.PostLogoutRedirectUri.IsNullOrEmpty(), x => x.PostLogoutRedirectUris.Contains(input.PostLogoutRedirectUri))
            .WhereIf(input.StartLastModificationTime.HasValue, x => x.LastModificationTime >= input.StartLastModificationTime)
            .WhereIf(input.EndLastModificationTime.HasValue, x => x.LastModificationTime < input.EndLastModificationTime)
            .WhereIf(input.StartCreationTime.HasValue, x => x.CreationTime >= input.StartCreationTime)
            .WhereIf(input.EndCreationTime.HasValue, x => x.CreationTime < input.EndCreationTime)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.ClientId.Contains(input.Keyword))
            ;

        return query;
    }

    [HttpPost]
    public override async Task<ApplicationDto> CreateAsync(ApplicationCreateInput input)
    {
        await CheckCreatePolicyAsync(input);

        await CheckCreateAsync(input);

        var application = await ApplicationManager.CreateAsync(
             name: input.ClientId,
             type: input.ClientType,
             consentType: input.ConsentType,
             displayName: input.DisplayName,
             secret: input.ClientSecret,
             grantTypes: input.GrantTypes,
             scopes: input.Scopes,
             redirectUri: input.RedirectUri,
             postLogoutRedirectUri: input.PostLogoutRedirectUri,
             permissions: input.Permissions);

        var entity = await GetEntityByIdAsync(application.Id);

        return await MapToGetOutputDtoAsync(entity);
    }

    protected virtual async Task<OpenIddictApplicationModel> FindByClientIdAsync(string clientId)
    {
        return (await ApplicationManager.FindByClientIdAsync(clientId)).As<OpenIddictApplicationModel>();
    }

    [HttpPost]
    public override async Task<ApplicationDto> UpdateAsync(Guid id, ApplicationUpdateInput input)
    {
        await CheckUpdatePolicyAsync(id, input);

        var application = (await ApplicationManager.FindByIdAsync(id.ToString())).As<OpenIddictApplicationModel>();

        await ApplicationManager.UpdateAsync(
            application,
            type: input.ClientType,
            consentType: input.ConsentType,
            displayName: input.DisplayName,
            secret: input.ClientSecret,
            grantTypes: input.GrantTypes,
            scopes: input.Scopes,
            redirectUri: input.RedirectUri,
            postLogoutRedirectUri: input.PostLogoutRedirectUri,
            permissions: input.Permissions
            );

        var entity = await GetEntityByIdAsync(application.Id);

        return await MapToGetOutputDtoAsync(entity);
    }

    [HttpGet]
    public virtual async Task<ApplicationDto> GetByClientIdAsync(string cliendId)
    {
        await CheckGetPolicyAsync();

        var app = Assert.NotNull(await FindByClientIdAsync(cliendId), $"No such application,clientId:{cliendId}");

        return await GetAsync(app.Id);
    }

    protected override async Task DeleteByIdAsync(Guid id)
    {
        await ApplicationManager.DeleteAsync(await ApplicationManager.FindByIdAsync(id.ToString()));
    }

    [HttpPost]
    public virtual async Task DeleteByClientIdAsync(string cliendId)
    {
        var app = Assert.NotNull(await FindByClientIdAsync(cliendId), $"No such application,clientId:{cliendId}");

        await DeleteAsync(app.Id);
    }

    [HttpPost]
    public virtual async Task DeleteManyByClientIdAsync(List<string> cliendIds)
    {
        Assert.If(!cliendIds.IsAny(), "cliendIds is null");

        var idList = new List<Guid>();

        foreach (var cliendId in cliendIds)
        {
            var app = Assert.NotNull(await FindByClientIdAsync(cliendId), $"No such application,clientId:{cliendId}");
            idList.Add(app.Id);
        }

        await DeleteManyAsync(idList);
    }
}
