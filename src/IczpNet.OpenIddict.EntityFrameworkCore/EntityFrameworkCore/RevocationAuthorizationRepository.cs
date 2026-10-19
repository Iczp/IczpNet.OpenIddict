using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Tokens;
using AuthDb = Volo.Abp.OpenIddict.EntityFrameworkCore.IOpenIddictDbContext;

namespace IczpNet.OpenIddict.EntityFrameworkCore;

public class RevocationAuthorizationRepository(IDbContextProvider<AuthDb> contexts) : EfCoreOpenIddictAuthorizationRepository(contexts)
{
    public override async Task<long> PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        long count = 0;
        var cutoff = DateTime.UtcNow.AddMinutes(-5);
        var tokens = (await GetDbContextAsync()).Set<OpenIddictToken>();
        var authorizations = await GetQueryableAsync();
        while (true)
        {
            var ids = await authorizations.Where(a => a.CreationDate < date &&
                (a.Status != OpenIddictConstants.Statuses.Valid ||
                    (a.Type == OpenIddictConstants.AuthorizationTypes.AdHoc && !tokens.Any(t => t.AuthorizationId == a.Id))) &&
                !tokens.Any(t => t.AuthorizationId == a.Id && (!t.ExpirationDate.HasValue || t.ExpirationDate >= cutoff)))
                .OrderBy(a => a.Id).Take(200).Select(a => a.Id).ToListAsync(cancellationToken);
            if (ids.Count == 0) return count;
            count += await tokens.Where(t => t.AuthorizationId.HasValue && ids.Contains(t.AuthorizationId.Value) && t.ExpirationDate < cutoff).ExecuteDeleteAsync(cancellationToken);
            count += await authorizations.Where(a => ids.Contains(a.Id) && !tokens.Any(t => t.AuthorizationId == a.Id)).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
