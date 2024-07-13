using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace IczpNet.OpenIddict.Applications;

public interface IClientSecretGenerator : ITransientDependency
{
    string Generate(int length = 32);

    Task<string> GenerateAsync(int length = 32);
}
