param(
  [string] $configuration = 'Debug',
  [string] $msbuild
)

$ErrorActionPreference = "Stop"
Write-Host "-msbuild: $msbuild"
Write-Host "MSBUILD_EXE: $env:MSBUILD_EXE"

$artifacts = Join-Path $PSScriptRoot ../artifacts
$logsDirectory = Join-Path $artifacts logs
if ($IsWindows) {
    $sln = Join-Path $PSScriptRoot ../Microsoft.Maui.Packages.slnf
} else {
    $sln = Join-Path $PSScriptRoot ../Microsoft.Maui.Packages-mac.slnf
}

# Build with dotnet
& dotnet pack $sln `
	-c:$configuration `
	-p:SymbolPackageFormat=snupkg `
	-bl:$logsDirectory/maui-pack-$configuration.binlog
if (!$?) { throw "Pack failed." }