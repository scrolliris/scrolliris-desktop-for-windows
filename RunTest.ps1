#!/usr/bin/env powershell -File

param(
  [string] $platform,
  [switch] $clean
)

$projectname = "Wetzikon.Tests"
$displayname = "Scrolliris Desktop for Windows (Tests)"
$packagename = "Scrolliris.Desktop.Windows.Tests"
$configuration = "Debug"
$version = "1.0.0.0"

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

Write-Host
Write-Host "Configuration: $configuration"
Write-Host "Platform: $platform"
Write-Host "Clean: $clean"
Write-Host

taskkill /im 'MSBuild.exe' /f
taskkill /im "testrunner.exe" /f

$currentDir = (Get-Item ".\").FullName
$packageDir = "${currentDir}\${projectname}" + `
  "\AppPackages\${projectname}_${version}_${platform}_${configuration}_Test"
$binDir = "${currentDir}\${projectname}\bin\${platform}\${configuration}"

# Clean
if ($clean) {
  wsl rm -f `
    "${projectname}/{bin,obj}/${platform}/${configuration}/${displayname}.exe"
  wsl rm -fr "${projectname}/{bin,obj}/${platform}/${configuration}/*"
  wsl rm -fr "${packageDir}/*"

  Remove-Item "${packageDir}" -Recurse -Force

  MSBuild.exe .\"${projectname}"\"${projectname}".csproj /t:Clean
  MSBuild.exe .\"${projectname}"\"${projectname}".csproj /t:Restore
}


# Build
MSBuild.exe .\"${projectname}"\"${projectname}".csproj /t:Build `
  /p:Configuration="${configuration}" `
  /p:Platform="${platform}"

if ($lastexitcode -ne 0) {
  Write-Host "Build faild with status: ${lastexitcode}"
  exit
}


# Run tests with vstest

# NOTE:
# It seems that `vstest.console.exe` binaries in following directories won't
# run correctly, appearently. Because it depends apparently VS :'(

$vsDir = "C:\Program Files (x86)\Microsoft Visual Studio\2017"
#$toolsDir = "${vsDir}\TestAgent\Common7\IDE"
#$toolsDir = "${vsDir}\BuildTools\Common7\IDE"
$toolsDir = "${vsDir}\Community\Common7\IDE"

# NOTE:
# What the difference of these? ... :'(

#$vstest = "${toolsDir}\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
$vstest = "${toolsDir}\Extensions\TestPlatform\vstest.console.exe"

Write-Host

& "$vstest" `
  /Platform:x64 `
  /Framework:FrameworkUap10 `
  /TestAdapterPath:"${projectname}\bin\${platform}\${configuration}" `
  "${binDir}\${projectname}.build.appxrecipe" `
  /Diag:VSTest.Console.log
