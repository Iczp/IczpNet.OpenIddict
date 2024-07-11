using FluentValidation;
using IczpNet.OpenIddict.Scopes.Dtos;
using IczpNet.OpenIddict.Localization;
using System;
using Volo.Abp.Localization;

namespace IczpNet.OpenIddict.Scopes;

public class ScopeInputValidator<T> : AbstractValidator<T> where T : ScopeUpdateInput
{
    public ScopeInputValidator()
    {
        
    }

    protected virtual bool BeAValidType(string type)
    {
        return OpenIddictConsts.ClientTypes.Contains(type);
    }
    protected virtual bool BeAValidConsentType(string type)
    {
        return OpenIddictConsts.ConsentTypes.Contains(type);
    }

    protected virtual bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    protected static LocalizableString L(string name)
    {
        return LocalizableString.Create<OpenIddictResource>(name);
    }
}
