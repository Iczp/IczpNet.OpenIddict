using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace IczpNet.OpenIddict.Applications;

public class ClientSecretGenerator : DomainService, IClientSecretGenerator
{

    protected static readonly char[] AvailableChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    public static string GenerateClientSecret2(int length = 32)
    {
        using var rng = RandomNumberGenerator.Create();
        var byteArray = new byte[length];
        var chars = new char[length];

        rng.GetBytes(byteArray);

        for (int i = 0; i < length; i++)
        {
            chars[i] = AvailableChars[byteArray[i] % AvailableChars.Length];
        }

        return new string(chars);
    }

    protected virtual string GenerateClientSecret(int length = 32)
    {
        using var rng = RandomNumberGenerator.Create();

        var byteArray = new byte[length];

        rng.GetBytes(byteArray);

        return Convert.ToBase64String(byteArray);
    }

    public virtual string Generate(int length = 32)
    {
        return GenerateClientSecret(length);
    }

    public virtual Task<string> GenerateAsync(int length = 32)
    {
        return Task.FromResult(Generate(length));
    }
}
