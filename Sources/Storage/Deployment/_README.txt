
*************************************************
*  It is instruction how to install databases   *
*************************************************

1. Create MSSQL user:  User name = CorbisUser; Password = CorbisUserPassword. If user exists skip this step


		--- CORBIS.MAIN & CORBIS.LOG ---

2. Create copy of DbDeployment.bat.local file (copy and delete '.local' postfix): DBDeployment.bat 
3. Edit file DbDeployment.bat: set 'sqlServer', 'sqlUser', 'sqlUserPassword'. Other variable must be used by default.
4. Run batch file. DbDeployment.bat installs databases Corbis.Main and Corbis.Log
5. Set CorbisUser as owners for created databases Corbis.Main and Corbis.Log
6. Use this user in your config files in connection strings.

		--- CORBIS.ASPSTATE ---
7. Use DbSessionSetup.bat.local bat file for session database installation




