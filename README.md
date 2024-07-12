# IczpNet.OpenIddict

An abp module that provides standard tree structure entity implement.

### Create project by Abp Cli

```bash
abp new IczpNet.OpenIddict -t module --no-ui -v 7.3.0
```

Add Moudule Volo.OpenIddict

```bash
abp add-module Volo.OpenIddict
```

#  FluentValidation 集成

```bash
abp add-package Volo.Abp.FluentValidation
```



### Build

```
dotnet build --configuration Release
```

https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build

### Public to Nuget.org

```
dotnet nuget push "src/*/bin/Release/*0.1.1.nupkg" --skip-duplicate -k {APIKEY} --source https://api.nuget.org/v3/index.json
```

## Dependency

### Volo.Abp.Identity

```bash
abp add-module Volo.Abp.Identity
```



### Volo.AuditLogging

```bash
abp add-module Volo.AuditLogging
```

## Usage

### Api : `xxx.HttpApi.Host/xxHttpApiHostModule`

```c#
 Configure<AbpAspNetCoreMvcOptions>(options =>
 {
     options.ConventionalControllers.Create(typeof(OpenIddictApplicationModule).Assembly);
 });
```



