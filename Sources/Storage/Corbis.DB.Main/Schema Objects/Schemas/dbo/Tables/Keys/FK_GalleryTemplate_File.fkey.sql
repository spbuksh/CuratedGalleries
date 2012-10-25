ALTER TABLE [dbo].[GalleryTemplate]
    ADD CONSTRAINT [FK_GalleryTemplate_File] FOREIGN KEY ([PackageID]) REFERENCES [dbo].[File] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;



