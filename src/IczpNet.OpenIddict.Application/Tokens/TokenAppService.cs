using IczpNet.AbpCommons.Dtos;
using IczpNet.OpenIddict.BaseAppServices;
using IczpNet.OpenIddict.Permissions;
using IczpNet.OpenIddict.Tokens.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Tokens;

namespace IczpNet.OpenIddict.Tokens;

public class TokenAppService : CrudOpenIddictAppService<OpenIddictToken, TokenDetailDto, TokenDto, Guid, TokenGetListInput, TokenCreateInput, TokenUpdateInput>, ITokenAppService
{

    protected override string GetListPolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.GetList;
    protected override string GetPolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.GetItem;
    //protected override string CreatePolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.Create;
    //protected override string UpdatePolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.Update;
    protected override string DeletePolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.Delete;
    protected virtual string GetStatusListPolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.GetStatusList;
    protected virtual string GetTypeListPolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.GetTypeList;

    protected ITokenManager TokenManager { get; set; }

    public TokenAppService(
        IRepository<OpenIddictToken, Guid> repository,
        ITokenManager tokenManager) : base(repository)
    {
        TokenManager = tokenManager;
    }


    protected override async Task<IQueryable<OpenIddictToken>> CreateFilteredQueryAsync(TokenGetListInput input)
    {
        var query = (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.Subject.IsNullOrEmpty(), x => x.Subject.Equals(input.Subject))
            .WhereIf(!input.ReferenceId.IsNullOrEmpty(), x => x.ReferenceId.Equals(input.ReferenceId))
            .WhereIf(!input.Status.IsNullOrEmpty(), x => x.Status.Equals(input.Status))
            .WhereIf(!input.Type.IsNullOrEmpty(), x => x.Type.Equals(input.Type))
            .WhereIf(input.ApplicationId.HasValue, x => x.ApplicationId.Equals(input.ApplicationId))
            .WhereIf(input.AuthorizationId.HasValue, x => x.AuthorizationId.Equals(input.AuthorizationId))
            //.WhereIf(input.StartLastModificationTime.HasValue, x => x.LastModificationTime >= input.StartLastModificationTime)
            //.WhereIf(input.EndLastModificationTime.HasValue, x => x.LastModificationTime < input.EndLastModificationTime)
            .WhereIf(input.StartCreationTime.HasValue, x => x.CreationDate >= input.StartCreationTime)
            .WhereIf(input.EndCreationTime.HasValue, x => x.CreationDate < input.EndCreationTime)

            .WhereIf(input.StartCreationDate.HasValue, x => x.CreationDate >= input.StartCreationDate)
            .WhereIf(input.EndCreationDate.HasValue, x => x.CreationDate < input.EndCreationDate)

            .WhereIf(input.StartRedemptionDate.HasValue, x => x.RedemptionDate >= input.StartRedemptionDate)
            .WhereIf(input.EndRedemptionDate.HasValue, x => x.RedemptionDate < input.EndRedemptionDate)

            .WhereIf(input.StartExpirationDate.HasValue, x => x.ExpirationDate >= input.StartExpirationDate)
            .WhereIf(input.EndExpirationDate.HasValue, x => x.ExpirationDate < input.EndExpirationDate)

            //.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword) || x.DisplayName.Contains(input.Keyword))
            ;

        return query;
    }

    /// <summary>
    /// Token Type List
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<PagedResultDto<KeyValueDto<string>>> GetTypeListAsync(TokenTypeGetListInput input)
    {
        return await GetEntityGroupListAsync(
            q => q.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Type.StartsWith(input.Keyword)),
            input, GetTypeListPolicyName, x => x.Type);
    }

    /// <summary>
    /// Token Status List
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<PagedResultDto<KeyValueDto<string>>> GetStatusListAsync(TokenStatusGetListInput input)
    {
        return await GetEntityGroupListAsync(
            q => q.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Status.StartsWith(input.Keyword)),
            input, GetStatusListPolicyName, x => x.Status);
    }

    [RemoteService(false)]
    public override Task<TokenDetailDto> CreateAsync(TokenCreateInput input)
    {
        //return base.CreateAsync(input);
        throw new NotImplementedException();
    }

    [RemoteService(false)]
    public override Task<TokenDetailDto> UpdateAsync(Guid id, TokenUpdateInput input)
    {
        //return base.UpdateAsync(id, input);
        throw new NotImplementedException();
    }

}
