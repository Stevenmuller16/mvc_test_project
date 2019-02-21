use master
DECLARE @dbname sysname
SET @dbname = 'nologodb'
DECLARE @spid int
SELECT @spid = min(spid) from master.dbo.sysprocesses where dbid = db_id(@dbname)
WHILE @spid IS NOT NULL
BEGIN
EXECUTE ('KILL ' + @spid)
SELECT @spid = min(spid) from master.dbo.sysprocesses where dbid = db_id(@dbname) AND spid > @spid
END
go
drop database [nologodb]
go

create database [nologodb]
go
use [nologodb]
go


CREATE TABLE [dbo].[UserAccount](
[UserAccountID] bigint identity primary key,
[UserAccountName] nvarchar(50),
[UserAccountPassword] nvarchar(50)
)
GO

SET IDENTITY_INSERT [dbo].[UserAccount] ON
INSERT [dbo].[UserAccount] ([UserAccountID],[UserAccountName],[UserAccountPassword]) VALUES(1,'test','test')
SET IDENTITY_INSERT [dbo].[UserAccount] OFF
GO


/****** Object:  Table [dbo].[Tournament]******/
CREATE TABLE [dbo].[Post](
[PostID] bigint identity primary key,
[FK_UserID] bigint,
[PostTitle] nvarchar(200),
[PostDate] datetime,
[PostAuthor] nvarchar(200),
[PostBody] nvarchar(MAX),
[PostImage] nvarchar(255)
CONSTRAINT [FK_Post_User] FOREIGN KEY ([FK_UserID]) REFERENCES [dbo].[UserAccount] ([UserAccountID])

)
GO

BEGIN
SET IDENTITY_INSERT [dbo].[Post] ON
 INSERT [dbo].[Post] ([PostID],[PostTitle],[PostDate],[PostAuthor],[PostBody],[PostImage]) VALUES(1,'the wonderful cityscape',CAST('2013-04-24 04:00:00:000' AS DateTime),'Steven','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur','https://source.unsplash.com/VcN0iqtY16M')
 INSERT [dbo].[Post] ([PostID],[PostTitle],[PostDate],[PostAuthor],[PostBody],[PostImage]) VALUES(2,'Beautiful snowy bridge',CAST('2013-04-24 04:00:00:000' AS DateTime),'Steven','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur','https://source.unsplash.com/WB-tCcWcLYs')
 INSERT [dbo].[Post] ([PostID],[PostTitle],[PostDate],[PostAuthor],[PostBody],[PostImage]) VALUES(3,'Mountains and Glaciers',CAST('2013-04-24 04:00:00:000' AS DateTime),'Steven','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur','https://source.unsplash.com/HmCqzAGbV7Q')
 INSERT [dbo].[Post] ([PostID],[PostTitle],[PostDate],[PostAuthor],[PostBody],[PostImage]) VALUES(4,'A wonderful kitty',CAST('2013-04-24 04:00:00:000' AS DateTime),'Steven','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur','https://source.unsplash.com/qDspZEzH8Ys')
 INSERT [dbo].[Post] ([PostID],[PostTitle],[PostDate],[PostAuthor],[PostBody],[PostImage]) VALUES(5,'Majestic Elephant',CAST('2013-04-24 04:00:00:000' AS DateTime),'Steven','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur','https://source.unsplash.com/f0oe9P9Yixs')
 INSERT [dbo].[Post] ([PostID],[PostTitle],[PostDate],[PostAuthor],[PostBody],[PostImage]) VALUES(6,'Lovely Roadtrip',CAST('2013-04-24 04:00:00:000' AS DateTime),'Steven','Lorem ipsum dolor sit amet, consectetur adipiscing elit,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,  sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur  Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur  Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur','https://source.unsplash.com/ZRsJmpt9pNI')

SET IDENTITY_INSERT [dbo].[Post] OFF
END
GO


/****** Object:  Table [dbo].[Event]******/
CREATE TABLE [dbo].[Tags](
[TagID] bigint identity primary key,
[FK_PostID] bigint,
[Tag] nvarchar(100) not null,
CONSTRAINT [FK_Tags_Post] FOREIGN KEY ([FK_PostID]) REFERENCES [dbo].[Post] ([PostID])
	ON DELETE CASCADE
	ON UPDATE CASCADE
)
GO

CREATE PROCEDURE [dbo].[Insert_Post]
      @PostTitle NVARCHAR(200),
	  @PostDate datetime,     
	  @PostAuthor NVARCHAR(200),
	  @PostBody NVARCHAR(200),
	  @PostImage NVARCHAR(200)
AS
BEGIN
      SET NOCOUNT ON
	  IF EXISTS(SELECT PostID FROM Post WHERE PostTitle = @PostTitle)
       BEGIN
              SELECT -1 AS PostID -- Username exists.
       END

      ELSE
       BEGIN
              INSERT INTO [Post]
                        ([PostTitle],[PostDate],[PostAuthor],[PostBody],[PostImage])
              VALUES
                        (@PostTitle,@PostDate,@PostAuthor,@PostBody,@PostImage)             
              SELECT SCOPE_IDENTITY() AS PostID -- PostId                     
     END
END
GO

CREATE PROCEDURE [dbo].[Update_Post]
	  @PostID bigint,
      @PostTitle NVARCHAR(200),
	  @PostDate NVARCHAR(200),     
	  @PostAuthor NVARCHAR(200),
	  @PostBody NVARCHAR(200),
	  @PostImage NVARCHAR(200)
AS
BEGIN
      UPDATE [Post]
      SET [PostTitle] = @PostTitle, [PostDate] = @PostDate, [PostAuthor] = @PostAuthor, [PostBody] = @PostBody, [PostImage] = @PostImage
      WHERE [PostID] = @PostID                             
END
GO

CREATE PROCEDURE [dbo].[Delete_Post]
      @PostID bigint
AS
BEGIN
      DELETE FROM [Post]
      WHERE [PostID] = @PostID                            
END
GO


CREATE PROCEDURE [dbo].[Insert_User]
      @UserName NVARCHAR(50) ,
	  @Password NVARCHAR(50)
	    
	      
AS
BEGIN
      SET NOCOUNT ON
	  BEGIN
              INSERT INTO [UserAccount]
                        ([UserAccountName],[UserAccountPassword])

              VALUES
                        (@UserName,@Password)
			SELECT SCOPE_IDENTITY() AS UserAccountID                 
     END
END
GO

