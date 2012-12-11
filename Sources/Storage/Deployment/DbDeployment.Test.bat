@echo OFF

SET sqlServer=".\MSSQL2008"
SET sqlUser=""
SET sqlUserPassword=""



SET logDB="Corbis.Log.Test"
SET logBinariesDirectory="..\Corbis.DB.Log\sql\debug\"
SET logVars=""
SET logSchemaPath="..\Corbis.DB.Log\sql\debug\Corbis.DB.Log.dbschema"

ECHO ----------- CREATING DATABASE: %logDB% ------------------
if %sqlUser%=="" (
    sqlcmd -S %sqlServer% -E -i "CreateDatabase.sql" -v DatabaseName = %logDB%
) ELSE (
    sqlcmd -S %sqlServer% -U %sqlUser% -P %sqlUserPassword% -i "CreateDatabase.sql" -v DatabaseName = %logDB%
)
IF ERRORLEVEL 1 exit /b %ERRORLEVEL%

ECHO ----------- APPLYING SCHEMA: %logDB% ------------------
call "DbSchemaDeployment.bat" %sqlServer% %sqlUser% %sqlUserPassword% %logDB% %logSchemaPath% %logVars%
IF ERRORLEVEL 1 exit /b %ERRORLEVEL%




SET mainDB="Corbis.Main.Test"
SET mainBinariesDirectory="..\Corbis.DB.Main\sql\debug\"
SET mainVars=""
SET mainSchemaPath="..\Corbis.DB.Main\sql\debug\Corbis.DB.Main.dbschema"
SET mainSeedScriptPath="..\Corbis.DB.Main\sql\debug\Corbis.DB.Main_Script.PostDeployment.sql"

ECHO ----------- CREATING DATABASE: %mainDB% ----------------
if %sqlUser%=="" (
    sqlcmd -S %sqlServer% -E -i "CreateDatabase.sql" -v DatabaseName = %mainDB% 
) ELSE (
    sqlcmd -S %sqlServer% -U %sqlUser% -P %sqlUserPassword% -i "CreateDatabase.sql" -v DatabaseName = %mainDB% 
)
IF ERRORLEVEL 1 exit /b %ERRORLEVEL%

ECHO ----------- APPLYING SCHEMA: %mainDB% ------------------
call "DbSchemaDeployment.bat" %sqlServer% %sqlUser% %sqlUserPassword% %mainDB% %mainSchemaPath% %mainVars%
IF ERRORLEVEL 1 exit /b %ERRORLEVEL%

ECHO ----------- INSERTING INITIAL DATA: %mainDB% ------------------
if %sqlUser%=="" (
    sqlcmd -S %sqlServer% -i %mainSeedScriptPath% -v DatabaseName=%mainDB%
) ELSE (
    sqlcmd -S %sqlServer% -U %sqlUser% -P %sqlUserPassword% -i %mainSeedScriptPath% -v DatabaseName=%mainDB%
)
IF ERRORLEVEL 1 exit /b %ERRORLEVEL%


exit /b %ERRORLEVEL%