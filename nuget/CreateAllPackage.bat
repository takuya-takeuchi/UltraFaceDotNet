dotnet restore ..\src\UltraFaceDotNet
dotnet build -c Release ..\src\UltraFaceDotNet

pwsh CreatePackage.ps1 CPU