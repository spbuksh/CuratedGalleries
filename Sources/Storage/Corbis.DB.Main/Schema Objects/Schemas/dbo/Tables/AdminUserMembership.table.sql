CREATE TABLE [dbo].[AdminUserMembership] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ProfileID]              INT            NOT NULL,
    [Login]                  NVARCHAR (100) NOT NULL,
    [Password]               NVARCHAR (150) NOT NULL,
    [PasswordExpirationDate] DATETIME2 (7)  NULL,
    [IsActive]               BIT            DEFAULT ((1)) NOT NULL,
    [DateCreated]            DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL
);

