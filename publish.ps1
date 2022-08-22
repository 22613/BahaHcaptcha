Set-Location $PSScriptRoot\src
dotnet publish -c release -f net472 -o bin\publish\net472
dotnet publish -c release -f net6.0-windows -o bin\publish\net6.0-windows

Set-Location bin\publish
Compress-Archive net472\* -DestinationPath BahaHcaptcha-net472.zip -f
Compress-Archive net6.0-windows\* -DestinationPath BahaHcaptcha-net6.0.zip -f
