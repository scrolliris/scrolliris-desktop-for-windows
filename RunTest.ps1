#!/usr/bin/env powershell -File

param([string]$platform)

$projectname = "Wetzikon.Tests"
$displayname = "Scrolliris Desktop for Windows (Tests)"
$packagename = "Scrolliris.Desktop.Windows.Tests"
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
# > powershell.exe -ExecutionPolicy Bypass -File .\RunTest.ps1 "x86"
# > powershell.exe -ExecutionPolicy Bypass -File .\RunTest.ps1 "x64"
# > powershell.exe -ExecutionPolicy Bypass -File .\RunTest.ps1 "ARM"
# ```

if ($platform -ne "x86" -and $platform -ne "x64" -and $platform -ne "ARM") {
  exit 1
}

taskkill /im 'MSBuild.exe' /f
taskkill /im "testrunner.exe" /f


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
  /p:Log="${log}"

if ($lastexitcode -ne 0) {
  Write-Host "Build faild with status: ${lastexitcode}"
  exit
}


# Run unit tests
$origin = $PWD
$location = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

Push-Location $location
[Environment]::CurrentDirectory = $location

# NOTE: Install TestRunner via `NuGet.exe install Wetzikon.Tests/packages.config`
$runnerVersion = "1.7.1"
$runner = "$location\Packages\TestRunner.${runnerversion}\tools\testrunner.exe"

& "$runner" "${binDir}\${displayname}.exe"

Pop-Location
[Environment]::CurrentDirectory = $origin
