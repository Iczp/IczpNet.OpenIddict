using IczpNet.AbpCommons.Permissions;
using IczpNet.OpenIddict.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace IczpNet.OpenIddict.Permissions;

public class OpenIddictPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OpenIddictPermissions.GroupName, L("Permission:OpenIddict"));

        myGroup.AddPermissions<OpenIddictPermissions>(x => L($"Permission:{x}"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OpenIddictResource>(name);
    }
}
