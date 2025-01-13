using FluentValidation;
using IczpNet.AbpCommons.Extensions;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.Localization;
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

        //RuleForEach(x => x.PostLogoutRedirectUris)
        //    .SetValidator(new UriValidator())
        //    .WithMessage("Each PostLogoutRedirectUri must be a valid URL.");

        RuleForEach(x => x.PostLogoutRedirectUris)
            .Must(BeAValidUrl)
            .When(x => x.PostLogoutRedirectUris.IsAny())
            .WithMessage("Each PostLogoutRedirectUris must be a valid URL when is not null.");

        RuleForEach(x => x.RedirectUris)
            .Must(BeAValidUrl)
            //.When(x => !string.IsNullOrEmpty(x.PostLogoutRedirectUris))
            .When(x => x.RedirectUris.IsAny())
            .WithMessage("Each RedirectUris must be a valid URL when is not null.");


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
