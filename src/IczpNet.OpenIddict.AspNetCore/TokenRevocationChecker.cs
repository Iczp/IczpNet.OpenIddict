using System.Security.Claims;
using IczpNet.OpenIddict.Tokens;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.DependencyInjection;

namespace IczpNet.OpenIddict;

public sealed class TokenRevocationChecker(IDistributedCache cache) : ITokenRevocationChecker, ITransientDependency
{
    public async Task<bool> IsTokenValidAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(principal.FindFirst(TokenRevocationKeys.TokenIdClaim)?.Value, out var tokenId)) return false;
        var authorization = principal.FindFirst(TokenRevocationKeys.AuthorizationIdClaim)?.Value;
        if (authorization != null && !Guid.TryParse(authorization, out _)) return false;

        // The independent reads are issued together so Redis can pipeline them.
        // This component has no Store, Repository, DbContext or authority HTTP client.
        try
        {
            var ready = cache.GetStringAsync(TokenRevocationKeys.Ready, cancellationToken);
            var token = cache.GetAsync(TokenRevocationKeys.Token(tokenId.ToString("D")), cancellationToken);
            var auth = authorization == null ? Task.FromResult<byte[]?>(null)
                : cache.GetAsync(TokenRevocationKeys.Authorization(authorization), cancellationToken);
            await Task.WhenAll(ready, token, auth);
            if (await ready != TokenRevocationKeys.ReadyValue) throw new TokenRevocationStateUnavailableException();
            return await token == null && await auth == null;
        }
        catch (TokenRevocationStateUnavailableException) { throw; }
        catch (Exception error) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TokenRevocationStateUnavailableException(error);
        }
    }
}
