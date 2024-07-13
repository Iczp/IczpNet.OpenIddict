using FluentValidation;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.Localization;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using Volo.Abp.Localization;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationInputValidator<T> : AbstractValidator<T> where T : ApplicationUpdateInput
{
    public ApplicationInputValidator()
    {
        RuleFor(x => x.ClientType)
        .NotEmpty()
        .WithMessage("Type is required.")
        .Must(BeAValidType)
        .WithMessage($"Type must be one of {OpenIddictConsts.ClientTypes.JoinAsString(",")}");

        RuleFor(x => x.ConsentType)
        .NotEmpty()
        .WithMessage("ConsentType is required.")
        .Must(BeAValidConsentType)
        .WithMessage($"ConsentType must be one of {OpenIddictConsts.ConsentTypes.JoinAsString(",")}");

        RuleFor(x => x.PostLogoutRedirectUri)
            .Must(BeAValidUrl)
            .When(x => !string.IsNullOrEmpty(x.PostLogoutRedirectUri))
            .WithMessage("PostLogoutRedirectUri must be a valid URL when is not null.");

        RuleFor(x => x.RedirectUri)
            .Must(BeAValidUrl)
            .When(x => !string.IsNullOrEmpty(x.PostLogoutRedirectUri))
            .WithMessage("RedirectUri must be a valid URL when is not null.");

        RuleFor(x => x.ClientSecret)
            .NotEmpty()
            .When(x => string.Equals(x.ClientType, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
            .WithMessage($"No client secret can be set for [clienType:{OpenIddictConstants.ClientTypes.Confidential}] applications.");

        RuleFor(x => x.ClientSecret)
            .Empty()
            .When(x => string.Equals(x.ClientType, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            .WithMessage($"Secret is required for [clienType:{OpenIddictConstants.ClientTypes.Public}] applications.");
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
