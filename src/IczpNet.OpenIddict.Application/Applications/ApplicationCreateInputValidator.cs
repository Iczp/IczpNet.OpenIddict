using FluentValidation;
using IczpNet.OpenIddict.Applications.Dtos;
using OpenIddict.Abstractions;
using System;

namespace IczpNet.OpenIddict.Applications;

public class ApplicationCreateInputValidator : ApplicationInputValidator<ApplicationCreateInput>
{
    public ApplicationCreateInputValidator()
    {

        RuleFor(x => x.ClientSecret)
            .NotEmpty()
            .When(x => string.Equals(x.ClientType, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
            .WithMessage($"No client secret can be set for [clienType:{OpenIddictConstants.ClientTypes.Confidential}] applications.");

        RuleFor(x => x.ClientSecret)
            .Empty()
            .When(x => string.Equals(x.ClientType, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            .WithMessage($"Secret is required for [clienType:{OpenIddictConstants.ClientTypes.Public}] applications.");
    }

}
