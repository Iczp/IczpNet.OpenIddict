using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace IczpNet.OpenIddict.Tokens;

public interface ITokenManager : IOpenIddictTokenManager, ITransientDependency
{
    Task<bool> IsTokenValidAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
    Task EnsureRevocationStateAsync(bool force = false, CancellationToken cancellationToken = default);
}
