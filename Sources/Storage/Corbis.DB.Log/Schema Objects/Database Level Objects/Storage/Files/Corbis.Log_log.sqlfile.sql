ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [Corbis.Log_log], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQL2008\MSSQL\DATA\Corbis.Log_log.LDF', SIZE = 832 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

