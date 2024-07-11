using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;

namespace IczpNet.OpenIddict.Applications;

public interface IApplicationManager : IAbpApplicationManager, ITransientDependency
{

    Task<string> SetClientSecretAsync(OpenIddictApplicationModel application, string secret, CancellationToken cancellationToken = default);

    ValueTask<OpenIddictApplicationModel> CreateApplicationAsync(
       [NotNull] string name,
       [NotNull] string type,
       [NotNull] string consentType,
       string displayName,
       string secret,
       List<string> grantTypes,
       List<string> scopes,
       string redirectUri = null,
       string postLogoutRedirectUri = null,
       List<string> permissions = null, CancellationToken cancellationToken = default);

    ValueTask<OpenIddictApplicationModel> UpdateApplicationAsync(
    [NotNull] Guid identifier,
    [NotNull] string type,
    [NotNull] string consentType,
    string displayName,
    string secret,
    List<string> grantTypes,
    List<string> scopes,
    string redirectUri = null,
    string postLogoutRedirectUri = null,
    List<string> permissions = null,
    CancellationToken cancellationToken = default);
    ValueTask<OpenIddictApplicationModel> UpdateApplicationAsync(
    OpenIddictApplicationModel applcation,
    [NotNull] string type,
    [NotNull] string consentType,
    string displayName,
    string secret,
    List<string> grantTypes,
    List<string> scopes,
    string redirectUri = null,
    string postLogoutRedirectUri = null,
    List<string> permissions = null,
    CancellationToken cancellationToken = default);
}
