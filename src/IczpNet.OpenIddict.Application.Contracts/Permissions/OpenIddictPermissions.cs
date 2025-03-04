﻿using Volo.Abp.Reflection;

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
        public const string GetTypeList = Default + "." + nameof(GetTypeList);
        public const string GetConsentTypeList = Default + "." + nameof(GetConsentTypeList);
        public const string GetClientSecret = Default + "." + nameof(GetClientSecret);
        public const string SetClientSecret = Default + "." + nameof(SetClientSecret);
        public const string ValidateClientSecret = Default + "." + nameof(ValidateClientSecret);
    }


    

    public class ScopePermissions
    {
        public const string Default = GroupName + "." + nameof(ScopePermissions);
        public const string GetItem = Default + "." + nameof(GetItem);
        public const string GetList = Default + "." + nameof(GetList);
        public const string Delete = Default + "." + nameof(Delete);
        public const string Update = Default + "." + nameof(Update);
        public const string Create = Default + "." + nameof(Create);
    }

    public class TokenPermissions
    {
        public const string Default = GroupName + "." + nameof(TokenPermissions);
        public const string GetItem = Default + "." + nameof(GetItem);
        public const string GetList = Default + "." + nameof(GetList);
        public const string Delete = Default + "." + nameof(Delete);
        //public const string Update = Default + "." + nameof(Update);
        //public const string Create = Default + "." + nameof(Create);
        public const string GetStatusList = Default + "." + nameof(GetStatusList);
        public const string GetTypeList = Default + "." + nameof(GetTypeList);
        public const string GetApplicationIdList = Default + "." + nameof(GetApplicationIdList);
    }

    public class AuthorizationPermissions
    {
        public const string Default = GroupName + "." + nameof(AuthorizationPermissions);
        public const string GetItem = Default + "." + nameof(GetItem);
        public const string GetList = Default + "." + nameof(GetList);
        public const string Delete = Default + "." + nameof(Delete);
        //public const string Update = Default + "." + nameof(Update);
        //public const string Create = Default + "." + nameof(Create);
    }
}
