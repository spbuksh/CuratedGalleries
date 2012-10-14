ALTER TABLE [dbo].[AdminUserToRole]
    ADD CONSTRAINT [FK_AdminUserToRole_AdminUserRole] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[AdminUserRole] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

