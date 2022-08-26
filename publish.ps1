Set-Location $PSScriptRoot\src
[xml]$csproj = Get-Content BahaHcaptcha.csproj
$version=$csproj.Project.PropertyGroup.Version

dotnet publish -c release -f net472 -o bin\publish\net472
dotnet publish -c release -f net6.0-windows -o bin\publish\net6.0-windows

Set-Location bin\publish
Compress-Archive net472\* -DestinationPath "BahaHcaptcha_v$($version)_net472.zip" -f
Compress-Archive net6.0-windows\* -DestinationPath "BahaHcaptcha_v$($version)_net6.0.zip" -f

explorer.exe /select,"BahaHcaptcha_v$($version)_net472.zip"
Set-Location $PSScriptRoot
