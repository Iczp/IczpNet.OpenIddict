using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IczpNet.OpenIddict.Tokens.Dtos;
using IczpNet.OpenIddict.BaseAppServices;
using IczpNet.OpenIddict.Permissions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Tokens;

namespace IczpNet.OpenIddict.Tokens;

public class TokenAppService : CrudOpenIddictAppService<OpenIddictToken, TokenDetailDto, TokenDto, Guid, TokenGetListInput, TokenCreateInput, TokenUpdateInput>, ITokenAppService
{

    protected override string GetListPolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.GetList;
    protected override string GetPolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.GetItem;
    protected override string CreatePolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.Create;
    protected override string UpdatePolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.Update;
    protected override string DeletePolicyName { get; set; } = OpenIddictPermissions.TokenPermissions.Delete;
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

            .WhereIf(input.StartLastModificationTime.HasValue, x => x.LastModificationTime >= input.StartLastModificationTime)
            .WhereIf(input.EndLastModificationTime.HasValue, x => x.LastModificationTime < input.EndLastModificationTime)
            .WhereIf(input.StartCreationTime.HasValue, x => x.CreationTime >= input.StartCreationTime)
            .WhereIf(input.EndCreationTime.HasValue, x => x.CreationTime < input.EndCreationTime)
            //.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword) || x.DisplayName.Contains(input.Keyword))
            ;

        return query;
    }

}
