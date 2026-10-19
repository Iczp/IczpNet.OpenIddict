using IczpNet.OpenIddict.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IczpNet.OpenIddict;

public sealed class TokenRevocationJwtOptions : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        var validated = options.Events.OnTokenValidated;
        var challenge = options.Events.OnChallenge;
        options.Events.OnTokenValidated = async context =>
        {
            try
            {
                if (context.Principal == null || !await context.HttpContext.RequestServices.GetRequiredService<ITokenRevocationChecker>()
                    .IsTokenValidAsync(context.Principal, context.HttpContext.RequestAborted))
                {
                    context.Fail("The token has been revoked or is no longer valid.");
                    return;
                }
            }
            catch (TokenRevocationStateUnavailableException error)
            {
                context.Fail(error);
                return;
            }
            await validated(context);
        };
        options.Events.OnChallenge = async context =>
        {
            if (context.AuthenticateFailure is TokenRevocationStateUnavailableException)
            {
                context.HandleResponse();
                context.Response.StatusCode = 503;
                context.Response.Headers.RetryAfter = "5";
                return;
            }
            await challenge(context);
        };
    }
}
