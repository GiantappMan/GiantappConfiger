@ECHO OFF
del *.nupkg
.\nuget.exe pack .\JsonConfiger.nuspec -OutputDirectory ..\..\LocalNuget\Packages -symbols

dotnet pack ../JsonConfiger.WPF -o ../../LocalNuget/Packages

