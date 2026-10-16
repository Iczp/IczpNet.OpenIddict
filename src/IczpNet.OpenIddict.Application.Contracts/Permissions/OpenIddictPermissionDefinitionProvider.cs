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

        // 对齐 ABP 官方原生 Client 权限提供者 ('C') 默认检测的 Policy 名称: OpenIddictPro.Application.ManagePermissions
        var proGroup = context.GetGroupOrNull(OpenIddictProPermissions.GroupName) ?? context.AddGroup(OpenIddictProPermissions.GroupName, L("Permission:OpenIddict"));
        proGroup.AddPermissions<OpenIddictProPermissions>(x => L($"Permission:{x}"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OpenIddictResource>(name);
    }
}
