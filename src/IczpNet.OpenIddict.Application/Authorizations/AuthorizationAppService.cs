using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IczpNet.OpenIddict.Authorizations.Dtos;
using IczpNet.OpenIddict.BaseAppServices;
using IczpNet.AbpCommons.Dtos;
using IczpNet.OpenIddict.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Authorizations;

namespace IczpNet.OpenIddict.Authorizations;

public class AuthorizationAppService : GetListOpenIddictAppService<OpenIddictAuthorization, AuthorizationDetailDto, AuthorizationDto, Guid, AuthorizationGetListInput>, IAuthorizationAppService
{

    protected override string GetListPolicyName { get; set; } = OpenIddictPermissions.AuthorizationPermissions.GetList;
    protected override string GetPolicyName { get; set; } = OpenIddictPermissions.AuthorizationPermissions.GetItem;
    //protected override string CreatePolicyName { get; set; } = OpenIddictPermissions.AuthorizationPermissions.Create;
    //protected override string UpdatePolicyName { get; set; } = OpenIddictPermissions.AuthorizationPermissions.Update;
    protected override string DeletePolicyName { get; set; } = OpenIddictPermissions.AuthorizationPermissions.Delete;
    protected virtual string GetStatusListPolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.GetStatusList;
    protected virtual string GetApplicationIdListPolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.GetApplicationIdList;

    protected IAuthorizationManager AuthorizationManager { get; set; }

    public AuthorizationAppService(
        IRepository<OpenIddictAuthorization, Guid> repository,
        IAuthorizationManager authorizationManager) : base(repository)
    {
        AuthorizationManager = authorizationManager;
    }


    protected override async Task<IQueryable<OpenIddictAuthorization>> CreateFilteredQueryAsync(AuthorizationGetListInput input)
    {
        var query = (await base.CreateFilteredQueryAsync(input))
            //.WhereIf(!input.Name.IsNullOrEmpty(), x => x.Name.Equals(input.Name))
            //.WhereIf(!input.Resources.IsNullOrEmpty(), x => x.Resources.Contains(input.Resources))
            .WhereIf(input.StartLastModificationTime.HasValue, x => x.LastModificationTime >= input.StartLastModificationTime)
            .WhereIf(input.EndLastModificationTime.HasValue, x => x.LastModificationTime < input.EndLastModificationTime)
            .WhereIf(input.StartCreationTime.HasValue, x => x.CreationTime >= input.StartCreationTime)
            .WhereIf(input.EndCreationTime.HasValue, x => x.CreationTime < input.EndCreationTime)
            //.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword) || x.DisplayName.Contains(input.Keyword))
            ;

        return query;
    }

    /// <summary>
    /// Authorization Status List
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<PagedResultDto<KeyValueDto<string>>> GetStatusListAsync(AuthorizationStatusGetListInput input)
    {
        return await GetEntityGroupListAsync(
            q => q.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Status.StartsWith(input.Keyword)),
            input, GetStatusListPolicyName, x => x.Status);
    }

    /// <summary>
    /// Authorization Status List
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<PagedResultDto<KeyValueDto<Guid?>>> GetApplicationIdListAsync(AuthorizationApplicationIdGetListInput input)
    {
        return await GetEntityGroupListAsync(
            q => q.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Status.StartsWith(input.Keyword)),
            input, GetApplicationIdListPolicyName, x => x.ApplicationId);
    }

}
