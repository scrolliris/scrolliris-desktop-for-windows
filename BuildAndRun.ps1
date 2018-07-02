#!/usr/bin/env powershell -File

param([string]$platform)

$projectname = "Wetzikon"
$displayname = "Scrolliris Desktop for Windows"
$packagename = "Scrolliris.Desktop.Windows"
$configuration = "Debug"
$log = "Trace"
$version = "0.0.1.0"

Write-Host ""
Write-Host "Configuration: $configuration"
Write-Host "Platform: $platform"
Write-Host "Log: $log"
Write-Host ""

# NOTE
#
# ```powershell
# > powershell.exe -ExecutionPolicy Bypass -File .\BuildAndRun.ps1 "x86"
# > powershell.exe -ExecutionPolicy Bypass -File .\BuildAndRun.ps1 "x64"
# > powershell.exe -ExecutionPolicy Bypass -File .\BuildAndRun.ps1 "ARM"
# ```

if ($platform -ne "x86" -and $platform -ne "x64" -and $platform -ne "ARM") {
  exit 1
}

taskkill /im 'MSBuild.exe' /f
taskkill /im "${displayname}.exe" /f

$currentDir = (Get-Item ".\").FullName
$packageDir = "${currentDir}\${projectname}\AppPackages"
$scriptPath = "${projectname}_${version}_${configuration}_Test" + `
  "\Add-AppDevPackage.ps1"


# Clean (resources and cache etc.)
wsl rm -f `
  "${projectname}/{bin,obj}/${platform}/${configuration}/${displayname}.exe"
wsl rm -fr "${projectname}/{bin,obj}/${platform}/${configuration}/*"

MSBuild.exe .\"${projectname}"\"${projectname}".csproj /t:Clean

rm "${packageDir}" -r -fo


# Restore
MSBuild.exe .\"${projectname}"\"${projectname}".csproj /t:Restore


# Build
MSBuild.exe .\"${projectname}"\"${projectname}".csproj /t:Build `
  /p:Configuration="${configuration}" `
  /p:Platform="${platform}" `
  /p:Log="${log}" `
  /p:AppxBundle=Always `
  /p:AppxBundlePlatforms="${platform}" `
  /p:UapAppxPackageBuildMode=StoreUpload

if ($lastexitcode -ne 0) {
  Write-Host
  Write-Host "Build faild with status: ${lastexitcode}"
  exit
}


# Register
Write-Host ""
Get-AppXPackage | findstr /i "${packagename}$"

if ($lastexitcode -eq 0) {
  Get-AppXPackage -Name "${packagename}" | Remove-AppxPackage
}

# https://docs.microsoft.com/en-us/previous-versions/windows/apps/hh454036(v=vs.140)#sideload-your-app-package
PowerShell.exe -ExecutionPolicy ByPass -Command `
  "& ${packageDir}\${scriptPath} -Force"

Write-Host ""
Get-AppXPackage | findstr /i "${packagename}$"
if ($lastexitcode -ne 0) {
  Write-Host
  Write-Host "Registration faild with status: ${lastexitcode}"
  exit
}

# Run
Write-Host ""
Write-Host "Application '${displayname}' is starting..."

explorer.exe shell:AppsFolder\$(Get-AppXPackage -name "$packagename" | `
  select -expandproperty PackageFamilyName)!App
