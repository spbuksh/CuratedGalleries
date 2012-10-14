::See details in http://msdn.microsoft.com/en-us/library/dd193283.aspx

SET sqlServer=%1
SET sqlUser=%2
SET sqlUserPassword=%3

SET dbName=%4
SET dbSchema=%5
SET dbPmtrs=%6

if %sqlUser%=="" (
    SET authInfo="Integrated Security=true;"
) ELSE (
    SET authInfo="User ID=%sqlUser%;Password=%sqlUserPassword%;"
)
SET authInfo=%authInfo:"=%

IF EXIST "%programfiles%\Microsoft Visual Studio 10.0\vstsdb\deploy\vsdbcmd.exe" (
    SET vsdbcmdPath="%programfiles%\Microsoft Visual Studio 10.0\vstsdb\deploy\vsdbcmd.exe"
) ELSE IF EXIST "%programfiles(x86)%\Microsoft Visual Studio 10.0\vstsdb\deploy\vsdbcmd.exe" (
    SET vsdbcmdPath="%programfiles(x86)%\Microsoft Visual Studio 10.0\vstsdb\deploy\vsdbcmd.exe"
) ELSE (
    echo "vsdbcmd.exe was not found" 1>&2
    exit /b 1
)

if %dbPmtrs%=="" (
    %vsdbcmdPath% /a:Deploy /cs:"Server=%sqlServer%;%authInfo%Pooling=false" /dsp:Sql /dd+ /model:%dbSchema% /p:TargetDatabase=%dbName% /p:AlwaysCreateNewDatabase=False
) ELSE (
    %vsdbcmdPath% /a:Deploy /cs:"Server=%sqlServer%;%authInfo%Pooling=false" /dsp:Sql /dd+ /model:%dbSchema% /p:TargetDatabase=%dbName% /p:SqlCommandVariablesFile=%dbPmtrs% /p:AlwaysCreateNewDatabase=False
)

exit /b %ERRORLEVEL%