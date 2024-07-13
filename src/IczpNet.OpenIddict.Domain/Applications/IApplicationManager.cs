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

    ValueTask<OpenIddictApplicationModel> CreateAsync(
       [NotNull] string name,
       [NotNull] string type,
       [NotNull] string consentType,
       string displayName,
       string secret,
       List<string> grantTypes,
       List<string> scopes,
       string redirectUri = null,
       string postLogoutRedirectUri = null,
       string clientUri = null,
       string logoUri = null,
       List<string> permissions = null, CancellationToken cancellationToken = default);

    ValueTask<OpenIddictApplicationModel> UpdateAsync(
    [NotNull] Guid identifier,
    [NotNull] string type,
    [NotNull] string consentType,
    string displayName,
    string secret,
    List<string> grantTypes,
    List<string> scopes,
    string redirectUri = null,
    string postLogoutRedirectUri = null,
    string clientUri = null,
    string logoUri = null,
    List<string> permissions = null,
    CancellationToken cancellationToken = default);

    ValueTask<OpenIddictApplicationModel> UpdateAsync(
    OpenIddictApplicationModel applcation,
    [NotNull] string type,
    [NotNull] string consentType,
    string displayName,
    string secret,
    List<string> grantTypes,
    List<string> scopes,
    string redirectUri = null,
    string postLogoutRedirectUri = null,
    string clientUri = null,
    string logoUri = null,
    List<string> permissions = null,
    CancellationToken cancellationToken = default);
}
