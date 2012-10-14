ALTER TABLE [dbo].[Portfolio]
    ADD CONSTRAINT [DF_Profile_IsActive] DEFAULT ((1)) FOR [IsActive];

