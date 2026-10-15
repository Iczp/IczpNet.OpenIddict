using IczpNet.AbpCommons;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Scopes;

namespace IczpNet.OpenIddict.Scopes;

public class ScopeManager : AbpScopeManager, IScopeManager
{


    protected IOpenIddictScopeManager OpenIddictScopeManager { get; set; }
    public ScopeManager(
        IOpenIddictScopeCache<OpenIddictScopeModel> cache,
        ILogger<OpenIddictScopeManager<OpenIddictScopeModel>> logger,
        IOptionsMonitor<OpenIddictCoreOptions> options,
        IOpenIddictScopeStore<OpenIddictScopeModel> store,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IOpenIddictScopeManager openIddictScopeManager) : base(cache, logger, options, store, identifierConverter)
    {
        OpenIddictScopeManager = openIddictScopeManager;
    }



    public virtual async Task<OpenIddictScopeModel> CreateAsync(
        string name,
        string displayName,
        List<string> resources,
        string description = null,
        List<string> claims = null,
        CancellationToken cancellationToken = default)
    {
        var scope = await OpenIddictScopeManager.FindByNameAsync(name, cancellationToken);

        Assert.If(scope != null, $"Scope name:'{name}' already exists.");

        var scopeDescriptor = new OpenIddictScopeDescriptor
        {
            Name = name,
            DisplayName = displayName,
            Description = description,
        };

        scopeDescriptor.Resources.Clear();

        if (resources != null)
        {
            foreach (var resource in resources)
            {
                if (scopeDescriptor.Resources.All(x => x != resource))
                {
                    scopeDescriptor.Resources.Add(resource);
                }
            }
        }

        if (claims != null && claims.Count > 0)
        {
            scopeDescriptor.Properties["Claims"] = System.Text.Json.JsonSerializer.SerializeToElement(claims);
        }

        return (await OpenIddictScopeManager.CreateAsync(scopeDescriptor, cancellationToken)).As<OpenIddictScopeModel>();
    }

    public async ValueTask<OpenIddictScopeModel> UpdateAsync(
        [NotNull] Guid identifier,
        [NotNull] string name,
        string displayName,
        List<string> resources,
        string description = null,
        List<string> claims = null,
        CancellationToken cancellationToken = default)
    {
        var scope = await OpenIddictScopeManager.FindByIdAsync(identifier.ToString(), cancellationToken);

        return await UpdateAsync(scope.As<OpenIddictScopeModel>(), name, displayName, resources, description, claims, cancellationToken);
    }

    public async ValueTask<OpenIddictScopeModel> UpdateAsync(
        OpenIddictScopeModel scope,
        [NotNull] string name,
        string displayName,
        List<string> resources,
        string description = null,
        List<string> claims = null,
        CancellationToken cancellationToken = default)
    {
        var descriptor = new OpenIddictScopeDescriptor();

        await PopulateAsync(descriptor, scope, cancellationToken);

        scope.DisplayName = displayName;
        descriptor.DisplayName = displayName;
        descriptor.Description = description;

        descriptor.Resources.Clear();

        if (resources != null)
        {
            foreach (var resource in resources)
            {
                if (descriptor.Resources.All(x => x != resource))
                {
                    descriptor.Resources.Add(resource);
                }
            }
        }

        if (claims != null)
        {
            descriptor.Properties["Claims"] = System.Text.Json.JsonSerializer.SerializeToElement(claims);
        }

        await PopulateAsync(scope, descriptor, cancellationToken);

        // 保存更改
        await UpdateAsync(scope, cancellationToken);

        return scope;
    }
}
