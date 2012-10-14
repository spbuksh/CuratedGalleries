CREATE TABLE [dbo].[AdminUserProfile] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]  NVARCHAR (50)  NOT NULL,
    [MiddleName] NVARCHAR (50)  NULL,
    [LastName]   NVARCHAR (50)  NOT NULL,
    [Email]      NVARCHAR (100) NULL
);

