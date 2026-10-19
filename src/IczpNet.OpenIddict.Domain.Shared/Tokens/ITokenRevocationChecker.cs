using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;

namespace IczpNet.OpenIddict.Tokens;

/// <summary>Checks an already cryptographically validated principal using shared revocation state.</summary>
public interface ITokenRevocationChecker
{
    Task<bool> IsTokenValidAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}

public static class TokenRevocationKeys
{
    public const string Prefix = "OpenIddict:Revocation:v2:";
    public const string Ready = Prefix + "ready";
    public const string ReadyValue = "2";
    public const string MutationLock = Prefix + "mutation";
    public const string TokenIdClaim = "oi_tkn_id";
    public const string AuthorizationIdClaim = "oi_au_id";
    public static string Token(string id) => Prefix + "token:" + System.Guid.Parse(id).ToString("D");
    public static string Authorization(string id) => Prefix + "authorization:" + System.Guid.Parse(id).ToString("D");
}

public class TokenRevocationStateUnavailableException : AbpException
{
    public TokenRevocationStateUnavailableException() : base("Token revocation state is being recovered. Retry later.") { }
    public TokenRevocationStateUnavailableException(System.Exception innerException)
        : base("Token revocation state is unavailable. Retry later.", innerException) { }
}
