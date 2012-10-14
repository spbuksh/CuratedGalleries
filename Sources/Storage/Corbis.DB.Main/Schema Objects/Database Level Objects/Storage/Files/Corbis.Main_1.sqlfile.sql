ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [Corbis.Main], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQL2008\MSSQL\DATA\Corbis.Main.mdf', SIZE = 2304 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];



