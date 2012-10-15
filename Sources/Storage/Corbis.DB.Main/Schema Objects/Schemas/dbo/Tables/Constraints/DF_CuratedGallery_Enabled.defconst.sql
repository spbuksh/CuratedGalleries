ALTER TABLE [dbo].[CuratedGallery]
    ADD CONSTRAINT [DF_CuratedGallery_Enabled] DEFAULT ((1)) FOR [Enabled];

