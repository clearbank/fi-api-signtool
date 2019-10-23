@ECHO OFF

SETLOCAL

SET CONFIGURATION=Release
IF NOT "%~1" == "" SET CONFIGURATION=%~1

dotnet publish -c %CONFIGURATION% -r win10-x64
REM dotnet publish -c %CONFIGURATION% -r ubuntu.16.10-x64
REM dotnet publish -c %CONFIGURATION% -r osx-x64
