#!/usr/bin/env powershell -File

param([string]$platform)

$projectname = "Wetzikon"
$displayname = "Scrolliris Desktop for Windows"
$packagename = "Scrolliris.Desktop.Windows"
$configuration = "Debug"
$log = "Trace"

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


# Clean (resources and cache etc.)
wsl rm -f `
  "${projectname}/{bin,obj}/${platform}/${configuration}/${displayname}.exe"
#wsl rm -fr "${projectname}/{bin,obj}/${platform}/${configuration}/*"

#MSBuild.exe .\"${projectname}"\"${projectname}".csproj /t:Clean


# Build
$binDir = ".\${projectname}\bin\${platform}\${configuration}"

MSBuild.exe .\"${projectname}"\"${projectname}".csproj /t:Build `
  /p:Configuration="${configuration}" `
  /p:Platform="${platform}" `
  /p:Log="${log}" `
  /p:AppxBundlePlatforms="${platform}" `
  /p:AppxBundle=Always `
  /p:UapAppxPackageBuildMode=StoreUpload

if ($lastexitcode -ne 0) {
  Write-Host "Build faild with status: ${lastexitcode}"
  exit
}


# Register
Write-Host ""
Get-AppXPackage | findstr /i "${packagename}"

if ($lastexitcode -ne 0) {
  Add-AppxPackage -Register "${binDir}\AppxManifest.xml"
}


# Run
Write-Host ""
Write-Host "Application '${displayname}' is starting..." -NoNewLine

Write-Host ""
explorer.exe shell:AppsFolder\$(Get-AppXPackage -name "$packagename" | `
  select -expandproperty PackageFamilyName)!App
