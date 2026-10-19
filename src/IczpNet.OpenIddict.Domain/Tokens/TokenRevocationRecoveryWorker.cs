using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace IczpNet.OpenIddict.Tokens;

/// <summary>Authority-side only. A cleared ready marker causes bounded database recovery.</summary>
public class TokenRevocationRecoveryWorker : AsyncPeriodicBackgroundWorkerBase
{
    public TokenRevocationRecoveryWorker(AbpAsyncTimer timer, IServiceScopeFactory scopes) : base(timer, scopes)
    {
        Timer.Period = 5000;
    }
    protected override async System.Threading.Tasks.Task DoWorkAsync(PeriodicBackgroundWorkerContext context)
        => await context.ServiceProvider.GetRequiredService<ITokenManager>().EnsureRevocationStateAsync(cancellationToken: context.CancellationToken);
}
