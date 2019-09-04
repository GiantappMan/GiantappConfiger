@ECHO OFF
del *.nupkg
.\nuget.exe pack .\GiantappConfiger.nuspec -OutputDirectory ..\..\LocalNuget\Packages -symbols

dotnet pack ../GiantappConfiger.WPF -o ../../LocalNuget/Packages

