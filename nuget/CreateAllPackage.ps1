$targets = @(
   "CPU",
   "GPU",
   "Xamarin"
)

$ScriptPath = $PSScriptRoot
$UltraFaceDotNetRoot = Split-Path $ScriptPath -Parent

$source = Join-Path $UltraFaceDotNetRoot src | `
          Join-Path -ChildPath UltraFaceDotNet
dotnet restore ${source}
# build for general
dotnet build -c Release ${source} /nowarn:CS1591

foreach ($target in $targets)
{
   pwsh CreatePackage.ps1 $target
}