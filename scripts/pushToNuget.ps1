

$nugetKeyFilePath = "../nuget_apikey.txt" 
$nugetSource = "https://api.nuget.org/v3/index.json" # NuGet 推送地址


# 从文件读取 NuGet API Key
if (Test-Path $nugetKeyFilePath) {
    $nugetApiKey = Get-Content $nugetKeyFilePath -ErrorAction Stop
    Write-Host "已成功读取 NuGet API Key: $nugetApiKey" -ForegroundColor Green
    Write-Host "推送地址: $nugetSource" -ForegroundColor Green
}
else {
    Write-Error "未找到 NuGet API Key 文件，请检查路径：$nugetKeyFilePath"
    exit 1
}


$defaultVersion = "9.0.0.0"
$newVersion = Read-Host "请输入新的版本号 (例如 $defaultVersion)"
if (-not $newVersion) {
    $newVersion = $defaultVersion
    Write-Host "使用版本号: $newVersion" -ForegroundColor Yellow
    # exit 1
}
Get-ChildItem -Path $projectsPath -Recurse -Filter *$newVersion.nupkg | ForEach-Object {
    $nupkgFile = $_.FullName
    # Write-Host "dotnet nuget push $nupkgFile --api-key $nugetApiKey --source $nugetSource" -ForegroundColor Green
    if ($?) {
        Write-Host " $nupkgFile" -ForegroundColor Green
    }
    else {
        Write-Error "推送失败: $nupkgFile" 
        exit 1
    }
}
Write-Host "dotnet build -c Release"

# 获取当前目录下的所有 .sln 文件
$solutionFiles = Get-Item -Path ".\*.sln"

# 遍历所有找到的解决方案文件并进行构建
foreach ($solutionFile in $solutionFiles) {
    Write-Host "正在构建: $($solutionFile.FullName)"
    dotnet build -c Release $solutionFile.FullName
    
    # 检查构建是否成功
    if ($LASTEXITCODE -ne 0) {
        Write-Error "构建失败，退出代码：$LASTEXITCODE，解决方案文件：$($solutionFile.FullName)"
    }
    else {
        Write-Host "构建成功：$($solutionFile.FullName)"
    }
}


Write-Host "项目构建成功！" -ForegroundColor Green

$confirmPush = Read-Host "是否推送版本[ $newVersion ]到 NuGet?(y/n)"
# dotnet nuget push "../src/*/bin/Release/*$newVersion.nupkg" --skip-duplicate -k $nugetKeyFilePath --source $nugetSource

# 如果用户没有输入或者输入的是空字符串，则默认设置为 "y"
if ([string]::IsNullOrWhiteSpace($confirmPush)) {
    $confirmPush = "y"
}

if ($confirmPush -eq "y") {
    Write-Host "开始推送到 NuGet 源:$nugetSource" -ForegroundColor Cyan
    # dotnet nuget push "../src/*/bin/Release/*$newVersion.nupkg" --skip-duplicate -k $nugetKeyFilePath --source $nugetSource

    Get-ChildItem -Path $projectsPath -Recurse -Filter *$newVersion.nupkg | ForEach-Object {
        $nupkgFile = $_.FullName
        Write-Host "dotnet nuget push $nupkgFile --api-key $nugetApiKey --source $nugetSource" -ForegroundColor Cyan
        # dotnet nuget push $nupkgFile --api-key $nugetApiKey --source $nugetSource
        if ($?) {
            Write-Host "推送成功 $nupkgFile" -ForegroundColor Green
        }
        else {
            Write-Error "推送失败: $nupkgFile" 
            exit 1
        }
    }
    Write-Host "所有包已成功推送到 NuGet 源。" -ForegroundColor Green

    Write-Host "查看 https://www.nuget.org/packages?q=IczpNet.OpenIddict" -ForegroundColor Green
    
}
else {
    Write-Host "推送到 NuGet 源已取消。" -ForegroundColor Yellow
}
