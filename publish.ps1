Set-Location $PSScriptRoot
[xml]$csproj = Get-Content .\src\BahaHcaptcha.csproj
$version=$csproj.Project.PropertyGroup.Version

dotnet publish src -c release -f net472 -o publish\net472
dotnet publish src -c release -f net6.0-windows -o publish\net6.0-windows
dotnet publish src -c release -f net6.0-windows -o publish\net6.0-windows-x64 -r win-x64 --self-contained=false -p:PublishSingleFile=true

Compress-Archive publish\net472\* -DestinationPath "publish\BahaHcaptcha_v$($version)_net472.zip" -f
Compress-Archive publish\net6.0-windows\* -DestinationPath "publish\BahaHcaptcha_v$($version)_net6.0.zip" -f
Compress-Archive publish\net6.0-windows-x64\* -DestinationPath "publish\BahaHcaptcha_v$($version)_net6.0_x64.zip" -f

explorer.exe /select,"BahaHcaptcha_v$($version)_net472.zip"
Set-Location $PSScriptRoot
