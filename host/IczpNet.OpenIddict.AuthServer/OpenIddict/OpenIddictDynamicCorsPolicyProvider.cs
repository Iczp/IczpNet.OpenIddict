using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;

namespace IczpNet.OpenIddict.OpenIddict;

public class OpenIddictDynamicCorsPolicyProvider : ICorsPolicyProvider
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly CorsOptions _options;

    public OpenIddictDynamicCorsPolicyProvider(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        IOptions<CorsOptions> options)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public virtual async Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
    {
        var origin = context.Request.Headers.Origin.ToString();
        if (string.IsNullOrEmpty(origin))
        {
            return _options.GetPolicy(policyName ?? _options.DefaultPolicyName);
        }

        // 1. 检查配置文件的静态 Origins
        var staticOrigins = _configuration["App:CorsOrigins"]?
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(o => o.TrimEnd('/'))
            .ToList() ?? new List<string>();

        if (staticOrigins.Any(o => string.Equals(o, origin, StringComparison.OrdinalIgnoreCase)))
        {
            return BuildPolicy(origin);
        }

        // 2. 检查缓存与数据库中注册的 Client AllowedCorsOrigins 和 RedirectUris
        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache<List<string>>>();

        var dynamicOrigins = await cache.GetOrAddAsync("OpenIddict:DynamicCorsOrigins", async () =>
        {
            var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            await foreach (var app in appManager.ListAsync())
            {
                // 从 RedirectUris 中提取 Origin
                var redirectUris = await appManager.GetRedirectUrisAsync(app);
                foreach (var uriStr in redirectUris)
                {
                    if (Uri.TryCreate(uriStr, UriKind.Absolute, out var uri))
                    {
                        set.Add($"{uri.Scheme}://{uri.Authority}");
                    }
                }

                // 从 Settings 中提取 AllowedCorsOrigins
                var settings = await appManager.GetSettingsAsync(app);
                if (settings.TryGetValue("AllowedCorsOrigins", out var corsOrigins) && !string.IsNullOrWhiteSpace(corsOrigins))
                {
                    var origins = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    foreach (var o in origins)
                    {
                        if (Uri.TryCreate(o, UriKind.Absolute, out var uri))
                        {
                            set.Add($"{uri.Scheme}://{uri.Authority}");
                        }
                        else
                        {
                            set.Add(o.TrimEnd('/'));
                        }
                    }
                }
            }

            return set.ToList();
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        if (dynamicOrigins != null && dynamicOrigins.Any(o => string.Equals(o, origin, StringComparison.OrdinalIgnoreCase)))
        {
            return BuildPolicy(origin);
        }

        return _options.GetPolicy(policyName ?? _options.DefaultPolicyName);
    }

    private static CorsPolicy BuildPolicy(string origin)
    {
        return new CorsPolicyBuilder()
            .WithOrigins(origin)
            .WithExposedHeaders("_AbpErrorFormat", "_AbpValidationErrors")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .Build();
    }
}
