CREATE TABLE [dbo].[File] (
    [ID]      BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]    NCHAR (50)      NULL,
    [Content] VARBINARY (MAX) NOT NULL
);

