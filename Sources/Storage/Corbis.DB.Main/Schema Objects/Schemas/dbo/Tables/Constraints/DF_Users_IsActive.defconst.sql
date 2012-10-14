ALTER TABLE [dbo].[AdminUsers]
    ADD CONSTRAINT [DF_Users_IsActive] DEFAULT ((1)) FOR [IsActive];

