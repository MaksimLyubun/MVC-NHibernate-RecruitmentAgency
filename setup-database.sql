CREATE DATABASE [RecruitmentAgency]
GO

USE [RecruitmentAgency];
CREATE TABLE [Users] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserName] [nvarchar] (255) NOT NULL UNIQUE,
    [PasswordHash] [nvarchar](255) NOT NULL DEFAULT (''),
	[UserRoleId] [int] NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [UserRoles] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar] (255) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [Summaries] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [JobseekerName] [nvarchar] (255) NOT NULL, 
    [DateOfBirth] [date] NOT NULL, 
    [Experience] [int] NOT NULL, 
    [Photo] [varbinary] (max) NULL, 
	[UserId] [int] NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [Vacancies] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar] (255) NOT NULL, 
    [Description] [nvarchar] (max) NULL, 
    [Term] [int] NULL, 
    [Company] [nvarchar] (255) NOT NULL, 
    [MinExperience] [int] NOT NULL, 
    [Salary] [int] NOT NULL, 
	[UserId] [int] NOT NULL,
	[Archived][BIT] NOT NULL DEFAULT(0),
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoles] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRoles] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Summaries]  WITH CHECK ADD  CONSTRAINT [FK_Summaries_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Vacancies]  WITH CHECK ADD  CONSTRAINT [FK_Vacancies_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

INSERT INTO [UserRoles] ([Name]) VALUES ('Администратор'), ('Соискатель'), ('Работодатель')
GO
 
/*Пароль "1" для каждого пользователя*/
INSERT INTO [dbo].[Users]
           ([UserName]
           ,[PasswordHash]
           ,[UserRoleId])
     VALUES
           ('admin', 'AOo0U7+Cud9Cm1eARF0AG4kKYePz0uPOWvsOCZjhhuNrAOO4Fw7cNQyBAhb7gF+dUA==', 1), 
           ('jobseeker', 'AOo0U7+Cud9Cm1eARF0AG4kKYePz0uPOWvsOCZjhhuNrAOO4Fw7cNQyBAhb7gF+dUA==', 2),
           ('employee', 'AOo0U7+Cud9Cm1eARF0AG4kKYePz0uPOWvsOCZjhhuNrAOO4Fw7cNQyBAhb7gF+dUA==', 3)
GO

 CREATE PROCEDURE [dbo].[AddVacancy]
			@Name nvarchar(255)
           ,@Description nvarchar(max)
           ,@Term int
           ,@Company nvarchar(255)
           ,@MinExperience int
           ,@Salary int
           ,@UserId int
           ,@Archived int
AS
BEGIN
	INSERT INTO [dbo].[Vacancies]
           ([Name]
           ,[Description]
           ,[Term]
           ,[Company]
           ,[MinExperience]
           ,[Salary]
           ,[UserId]
           ,[Archived])
     VALUES
           (@Name
           ,@Description
           ,@Term
           ,@Company
           ,@MinExperience
           ,@Salary
           ,@UserId
           ,@Archived)
END
GO
