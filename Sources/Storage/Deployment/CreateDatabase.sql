USE [master]

IF EXISTS(SELECT * FROM dbo.sysdatabases WHERE name = '$(DatabaseName)')
BEGIN
	ALTER DATABASE [$(DatabaseName)] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [$(DatabaseName)]
END

IF @@Error <> 0 RETURN

CREATE DATABASE [$(DatabaseName)] COLLATE Latin1_General_CI_AS

IF @@Error <> 0 RETURN

ALTER DATABASE [$(DatabaseName)] SET QUOTED_IDENTIFIER ON

IF @@Error <> 0 RETURN

ALTER DATABASE [$(DatabaseName)] SET ANSI_NULLS ON

IF @@Error <> 0 RETURN

GO
