using IczpNet.AbpCommons;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.PermissionManagement;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;
using Volo.Abp.DependencyInjection;
using System.Threading;
using IczpNet.AbpCommons.Extensions;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationManager : AbpApplicationManager, IApplicationManager
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; }
    protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

    protected virtual IStringLocalizer CreateLocalizer()
    {
        if (LocalizationResource != null)
        {
            return StringLocalizerFactory.Create(LocalizationResource);
        }

        var localizer = StringLocalizerFactory.CreateDefaultOrNull();
        if (localizer == null)
        {
            throw new AbpException($"Set {nameof(LocalizationResource)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
        }

        return localizer;
    }

    protected IStringLocalizer L
    {
        get
        {
            if (_localizer == null)
            {
                _localizer = CreateLocalizer();
            }

            return _localizer;
        }
    }

    private IStringLocalizer _localizer;

    protected Type LocalizationResource
    {
        get => _localizationResource;
        set
        {
            _localizationResource = value;
            _localizer = null;
        }
    }

    private Type _localizationResource = typeof(DefaultResource);

    protected IPermissionDataSeeder PermissionDataSeeder { get; set; }

    public ApplicationManager(
        IOpenIddictApplicationCache<OpenIddictApplicationModel> cache,
        ILogger<AbpApplicationManager> logger,
        IOptionsMonitor<OpenIddictCoreOptions> options,
        IOpenIddictApplicationStoreResolver resolver,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IPermissionDataSeeder permissionDataSeeder) : base(cache, logger, options, resolver, identifierConverter)
    {
        PermissionDataSeeder = permissionDataSeeder;
    }


    public virtual async Task<string> SetClientSecretAsync(OpenIddictApplicationModel application, string secret, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            await Store.SetClientSecretAsync(application, null, CancellationToken.None);
            return null;
        }
        var obfuscateClientSecret = await ObfuscateClientSecretAsync(secret, CancellationToken.None);
        await Store.SetClientSecretAsync(application, obfuscateClientSecret, CancellationToken.None);
        return obfuscateClientSecret;
    }

    private void CheckSecret(string type, string secret)
    {
        if (!string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException(L["NoClientSecretCanBeSetForPublicApplications"]);
        }

        if (string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException(L["TheClientSecretIsRequiredForConfidentialApplications"]);
        }
    }

    public virtual async ValueTask<OpenIddictApplicationModel> CreateAsync(
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
       List<string> permissions = null, CancellationToken cancellationToken = default)
    {
        Assert.If(string.IsNullOrEmpty(name), $"client_id  required");

        var client = await FindByClientIdAsync(name, cancellationToken);

        Assert.If(client != null, $"client_id:'{name}' is existed.");

        //if (!string.IsNullOrEmpty(name) && await ApplicationManager.FindByClientIdAsync(name) != null)
        //{
        //    throw new BusinessException(L["TheClientIdentifierIsAlreadyTakenByAnotherApplication"]);
        //}

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = name,
            Type = type,
            ClientSecret = secret,
            ConsentType = consentType,
            DisplayName = displayName,
        };

        CheckSecret(type, secret);

        Check.NotNullOrEmpty(grantTypes, nameof(grantTypes));

        Check.NotNullOrEmpty(scopes, nameof(scopes));

        SetLogoutPermission(descriptor.Permissions, !redirectUri.IsNullOrWhiteSpace() || !postLogoutRedirectUri.IsNullOrWhiteSpace());

        SetCodePermission(descriptor.Permissions, type, grantTypes);

        SetGrantTypes(descriptor.Permissions, type, grantTypes);

        SetScopes(descriptor.Permissions, scopes);

        SetUris(descriptor.RedirectUris, [redirectUri], "InvalidRedirectUri");

        SetUris(descriptor.PostLogoutRedirectUris, [postLogoutRedirectUri], "InvalidPostLogoutRedirectUri");

        await UpdatePermissionAsync(descriptor.ClientId, permissions);

        return (await CreateAsync(descriptor, cancellationToken)).As<OpenIddictApplicationModel>();

    }

    private void SetCodePermission(HashSet<string> permissions, string type, List<string> grantTypes)
    {
        if (new[] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }.All(grantTypes.Contains))
        {
            permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);

            if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
            }
        }
    }

    private void SetLogoutPermission(HashSet<string> permissions, bool isHas)
    {
        if (isHas)
        {
            permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);
        }
        else
        {
            permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Logout);
        }
    }

    private HashSet<string> SetScopes(HashSet<string> permissions, List<string> scopes)
    {
        var buildInScopes = new[]
        {
            OpenIddictConstants.Permissions.Scopes.Address,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles
        };

        foreach (var scope in scopes)
        {
            if (buildInScopes.Contains(scope))
            {
                permissions.Add(scope);
            }
            else
            {
                permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
            }
        }
        return permissions;
    }
    private HashSet<Uri> SetUris(HashSet<Uri> uris, List<string> urls, string localizer)
    {
        uris.Clear();

        foreach (var url in urls)
        {
            if (url.IsNullOrEmpty())
            {
                continue;
            }

            Assert.If(!Uri.TryCreate(url, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString(), L[localizer, url]);

            if (uris.All(x => x != uri))
            {
                uris.Add(uri);
            }
        }

        return uris;
    }

    private HashSet<string> SetGrantTypes(HashSet<string> permissions, string type, List<string> grantTypes)
    {
        if (!grantTypes.IsAny())
        {
            return permissions;
        }

        var buildInGrantTypes = new[]
        {
            OpenIddictConstants.GrantTypes.Implicit,
            OpenIddictConstants.GrantTypes.Password,
            OpenIddictConstants.GrantTypes.AuthorizationCode,
            OpenIddictConstants.GrantTypes.ClientCredentials,
            OpenIddictConstants.GrantTypes.DeviceCode,
            OpenIddictConstants.GrantTypes.RefreshToken
        };

        foreach (var grantType in grantTypes)
        {
            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode)
            {
                permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
            }

            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode || grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
            }

            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                grantType == OpenIddictConstants.GrantTypes.ClientCredentials ||
                grantType == OpenIddictConstants.GrantTypes.Password ||
                grantType == OpenIddictConstants.GrantTypes.RefreshToken ||
                grantType == OpenIddictConstants.GrantTypes.DeviceCode)
            {
                permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
            }

            if (grantType == OpenIddictConstants.GrantTypes.ClientCredentials)
            {
                permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Password)
            {
                permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
            }

            if (grantType == OpenIddictConstants.GrantTypes.RefreshToken)
            {
                permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
            }

            if (grantType == OpenIddictConstants.GrantTypes.DeviceCode)
            {
                permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
                if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                {
                    permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                    permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                }
            }

            if (!buildInGrantTypes.Contains(grantType))
            {
                permissions.Add(OpenIddictConstants.Permissions.Prefixes.GrantType + grantType);
            }
        }
        return permissions;
    }

    public virtual async ValueTask<OpenIddictApplicationModel> UpdateAsync(
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
    CancellationToken cancellationToken = default)
    {
        var applcation = await FindByIdAsync(identifier.ToString(), cancellationToken);

        return await UpdateAsync(applcation, type, consentType, displayName, secret, grantTypes, scopes, redirectUri, postLogoutRedirectUri, clientUri, logoUri, permissions, cancellationToken);
    }


    public virtual async ValueTask<OpenIddictApplicationModel> UpdateAsync(
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
    CancellationToken cancellationToken = default)
    {
        // 查找现有应用程序
        //var applcation = await FindByIdAsync(identifier.ToString(), cancellationToken);

        //Assert.If(name.Equals(applcation.ClientId), $"client_id cannot be modified");

        //CheckSecret(type, secret);

        CheckSecret(type, applcation.ClientSecret);

        Check.NotNullOrEmpty(grantTypes, nameof(grantTypes));

        Check.NotNullOrEmpty(scopes, nameof(scopes));

        Assert.If(applcation == null, $"The application is null.");



        var descriptor = new OpenIddictApplicationDescriptor
        {
            Type = type,
            ConsentType = consentType,
            DisplayName = displayName
        };

        applcation.Type = type;

        applcation.ConsentType = consentType;
        applcation.DisplayName = displayName;
        applcation.ClientUri = clientUri;
        applcation.LogoUri = logoUri;

        await PopulateAsync(descriptor, applcation, cancellationToken);

        descriptor.Permissions.Clear();

        SetLogoutPermission(descriptor.Permissions, !redirectUri.IsNullOrWhiteSpace() || !postLogoutRedirectUri.IsNullOrWhiteSpace());

        SetCodePermission(descriptor.Permissions, type, grantTypes);

        // 更新授权类型
        SetGrantTypes(descriptor.Permissions, type, grantTypes);

        // 更新作用域
        SetScopes(descriptor.Permissions, scopes);

        // 更新重定向 URI
        SetUris(descriptor.RedirectUris, [redirectUri], "InvalidRedirectUri");

        // 更新登出后重定向 URI
        SetUris(descriptor.PostLogoutRedirectUris, [postLogoutRedirectUri], "InvalidPostLogoutRedirectUri");

        // 更新权限
        await UpdatePermissionAsync(descriptor.ClientId, permissions);

        await PopulateAsync(applcation, descriptor, cancellationToken);

        //// 更新客户端密钥
        //await SetClientSecretAsync(applcation, secret, cancellationToken);

        // 保存更改
        await UpdateAsync(applcation, cancellationToken);

        return applcation;
    }

    /// <summary>
    /// 更新权限
    /// </summary>
    private async Task UpdatePermissionAsync(string providerKey, IEnumerable<string> grantedPermissions)
    {
        // 更新权限
        if (grantedPermissions != null)
        {
            await PermissionDataSeeder.SeedAsync(
                ClientPermissionValueProvider.ProviderName,
                providerKey,
                grantedPermissions,
                null
            );
        }
    }

    public override async ValueTask<bool> ValidateClientSecretAsync(OpenIddictApplicationModel application, string secret, CancellationToken cancellationToken = default)
    {
        if (application.Type.Equals(OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase) && secret == null)
        {
            return true;
        }
        return await base.ValidateClientSecretAsync(application, secret, cancellationToken);
    }

}
