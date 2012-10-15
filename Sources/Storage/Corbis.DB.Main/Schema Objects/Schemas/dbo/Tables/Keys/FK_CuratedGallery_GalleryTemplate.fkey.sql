ALTER TABLE [dbo].[CuratedGallery]
    ADD CONSTRAINT [FK_CuratedGallery_GalleryTemplate] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[GalleryTemplate] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

