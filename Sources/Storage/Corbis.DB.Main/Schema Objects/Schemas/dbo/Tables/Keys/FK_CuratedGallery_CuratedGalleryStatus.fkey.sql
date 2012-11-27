ALTER TABLE [dbo].[CuratedGallery]
    ADD CONSTRAINT [FK_CuratedGallery_CuratedGalleryStatus] FOREIGN KEY ([StatusID]) REFERENCES [dbo].[CuratedGalleryStatus] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

