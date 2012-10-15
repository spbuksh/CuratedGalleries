CREATE TABLE [dbo].[CuratedGallery] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [StatusID]     INT           NOT NULL,
    [Enabled]      BIT           NOT NULL,
    [TemplateID]   INT           NOT NULL,
    [DateCreated]  DATETIME2 (7) NOT NULL,
    [DateModified] DATETIME2 (7) NULL,
    [Archive]      BIGINT        NULL
);

