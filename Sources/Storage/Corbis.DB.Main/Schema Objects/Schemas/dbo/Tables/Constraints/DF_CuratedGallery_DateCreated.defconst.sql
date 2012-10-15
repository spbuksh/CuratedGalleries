ALTER TABLE [dbo].[CuratedGallery]
    ADD CONSTRAINT [DF_CuratedGallery_DateCreated] DEFAULT (getutcdate()) FOR [DateCreated];

