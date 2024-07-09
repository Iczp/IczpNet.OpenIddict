using Volo.Abp.Reflection;

namespace IczpNet.OpenIddict.Permissions;

public class OpenIddictPermissions
{
    public const string GroupName = "OpenIddict";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(OpenIddictPermissions));
    }
}
