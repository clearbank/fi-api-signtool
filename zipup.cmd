@ECHO OFF

SETLOCAL

SET ZIPFILE=FI.API.SignTool.zip

PUSHD "%~dp0"

IF EXIST "%ZIPFILE%" DEL "%ZIPFILE%" > NUL

@ECHO ON
ZIP -r --exclude=*\.vs\* --exclude=*\bin\* --exclude=*\obj\* --exclude=*.args.json --exclude=*.user --exclude=*%~nx0 "%ZIPFILE%" *.*
@ECHO OFF

POPD
