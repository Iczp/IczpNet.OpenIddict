using JetBrains.Annotations;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;

namespace IczpNet.OpenIddict.Scopes;

public interface IScopeManager : IOpenIddictScopeManager, ITransientDependency
{

    Task<OpenIddictScopeModel> CreateAsync([NotNull] string name, string displayName, List<string> resources, CancellationToken cancellationToken = default);

    ValueTask<OpenIddictScopeModel> UpdateAsync(
    [NotNull] Guid identifier,
    [NotNull] string name,
    string displayName,
    List<string> resources,
    CancellationToken cancellationToken = default);

    ValueTask<OpenIddictScopeModel> UpdateAsync(
    OpenIddictScopeModel scope,
    [NotNull] string name,
    string displayName,
    List<string> resources,
    CancellationToken cancellationToken = default);
}
