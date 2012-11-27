/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

USE [$(DatabaseName)]
GO
	/***************** [dbo].[CuratedGalleryStatus] table */
	DELETE FROM [dbo].[CuratedGalleryStatus]

	INSERT INTO [dbo].[CuratedGalleryStatus] ([Name])
		 VALUES (N'Published')

	INSERT INTO [dbo].[CuratedGalleryStatus] ([Name])
		 VALUES (N'UnPublished')

	PRINT N'Table [dbo].[CuratedGalleryStatus] is filled with values successfully'

	/***************** [dbo].[AdminUserRole] table */
	DELETE FROM [dbo].[AdminUserRole]

	-- NOTE: Role identifiers are FLAGs!!! It means if role includes any other then super role contains these roles via logical OR
	-- Regular admin role
	INSERT INTO [dbo].[AdminUserRole] ([ID], [Name], [Description])
		 VALUES (1, N'Regular', N'Regular admin role')

	INSERT INTO [dbo].[AdminUserRole] ([ID], [Name], [Description])
		 VALUES (3, N'Super', N'Super admin role. It includes regular admin role')

	PRINT N'Table [dbo].[AdminUserRole] is filled with values successfully'


	/***************** [dbo].[AdminUserProfile] table */
	DELETE FROM [dbo].[AdminUserProfile]

	-- Active Regular admin
	INSERT INTO [dbo].[AdminUserProfile] ([FirstName], [MiddleName], [LastName], [Email])
		 VALUES (N'John', NULL, N'Doe', N'john.doe.admin@soft.com')

	INSERT INTO [dbo].[AdminUserMembership] ([ProfileID], [Login], [Password], [IsActive])
		VALUES(1, N'john.doe', N'3JYV2df54q8sS4Ztu0bqFLPHRBZX549ZqUpLvSQ0CFw=', 1)

	INSERT INTO [dbo].[AdminUserToRole] ([MemberID], [RoleID])
		VALUES(1, 1)

	-- Active Regular admin
	INSERT INTO [dbo].[AdminUserProfile] ([FirstName], [MiddleName], [LastName], [Email])
		 VALUES (N'Ivan', NULL, N'Petrov', N'ivan.petrov.admin@soft.com')

	INSERT INTO [dbo].[AdminUserMembership] ([ProfileID], [Login], [Password], [IsActive])
		VALUES(2, N'ivan.petrov', N'ji+hdD4Miciuo/pRWq2B/s9eUOJCS7jasj1E0R1ZOwk=', 0)

	INSERT INTO [dbo].[AdminUserToRole] ([MemberID], [RoleID])
		VALUES(2, 1)
		
	-- Super admin
	INSERT INTO [dbo].[AdminUserProfile] ([FirstName], [MiddleName], [LastName], [Email])
		 VALUES (N'Adam', NULL, N'Smith', N'adam.smith.superadmin@soft.com')
	    
	INSERT INTO [dbo].[AdminUserMembership] ([ProfileID], [Login], [Password], [IsActive])
		VALUES(3, N'adam.smith', N'MBPCMfVt8SpCj9GsCEU1n+I2s7poS7LCH1P4722ugKY=', 1)
	    
	INSERT INTO [dbo].[AdminUserToRole] ([MemberID], [RoleID])
		VALUES(3, 3)	
