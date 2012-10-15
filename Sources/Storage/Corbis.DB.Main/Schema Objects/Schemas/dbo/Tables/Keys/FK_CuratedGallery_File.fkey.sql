ALTER TABLE [dbo].[CuratedGallery]
    ADD CONSTRAINT [FK_CuratedGallery_File] FOREIGN KEY ([Archive]) REFERENCES [dbo].[File] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

