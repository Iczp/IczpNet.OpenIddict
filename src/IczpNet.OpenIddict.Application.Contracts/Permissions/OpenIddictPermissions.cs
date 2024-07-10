using Volo.Abp.Reflection;

namespace IczpNet.OpenIddict.Permissions;

public class OpenIddictPermissions
{
    public const string GroupName = "OpenIddict";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(OpenIddictPermissions));
    }

    public class ApplicationPermissions
    {
        public const string Default = GroupName + "." + nameof(ApplicationPermissions);
        public const string GetItem = Default + "." + nameof(GetItem);
        public const string GetList = Default + "." + nameof(GetList);
        public const string Delete = Default + "." + nameof(Delete);
        public const string Update = Default + "." + nameof(Update);
        public const string Create = Default + "." + nameof(Create);
    }
}
