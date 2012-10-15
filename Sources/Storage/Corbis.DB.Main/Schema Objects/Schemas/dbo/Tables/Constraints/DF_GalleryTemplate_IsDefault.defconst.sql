ALTER TABLE [dbo].[GalleryTemplate]
    ADD CONSTRAINT [DF_GalleryTemplate_IsDefault] DEFAULT ((0)) FOR [IsDefault];

