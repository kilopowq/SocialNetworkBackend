USE master
IF EXISTS (SELECT * FROM sys.databases WHERE name='NewSNS')
	DROP DATABASE NewSNS
GO

CREATE DATABASE NewSNS;
GO

USE NewSNS;
GO


--
--
--
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblUserState](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](20) NOT NULL,
 CONSTRAINT [PK_tblUserState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


--
--
--
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StateId] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](92) NOT NULL,
	[BirthDate] [date] NULL,
	[Email] [nvarchar](50) NULL,
	[Info] [nvarchar](100) NULL,
	[Country] [nvarchar](20) NULL,
	[City] [nvarchar](30) NULL,
	[Phone] [nvarchar](15) NULL,
	[Avatar] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblUser]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_tblUserState] FOREIGN KEY([StateId])
REFERENCES [dbo].[tblUserState] ([Id])
GO

ALTER TABLE [dbo].[tblUser] CHECK CONSTRAINT [FK_tblUser_tblUserState]
GO



--
--
--
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblFriend](
	[UserID] [int] NOT NULL,
	[FriendID] [int] NOT NULL,
	[StatusFriendship] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblFriend]  WITH CHECK ADD  CONSTRAINT [FK_tblFriend_tblUser_FKFriendID] FOREIGN KEY([FriendID])
REFERENCES [dbo].[tblUser] ([Id])
GO

ALTER TABLE [dbo].[tblFriend] CHECK CONSTRAINT [FK_tblFriend_tblUser_FKFriendID]
GO

ALTER TABLE [dbo].[tblFriend]  WITH CHECK ADD  CONSTRAINT [FK_tblFriend_tblUser_FKUserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[tblUser] ([Id])
GO

ALTER TABLE [dbo].[tblFriend] CHECK CONSTRAINT [FK_tblFriend_tblUser_FKUserID]
GO


--
--
--
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblMessage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[Location] [nvarchar](20) NULL,
	[ConferenceId] [int] NULL,
 CONSTRAINT [PK_tblMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblMessage]  WITH CHECK ADD  CONSTRAINT [FK_tblMessage_tblUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[tblUser] ([Id])
GO

ALTER TABLE [dbo].[tblMessage] CHECK CONSTRAINT [FK_tblMessage_tblUser]
GO


--
--
--
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblConference](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Photo] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tblConference] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO




--
--
--
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblTimeline](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageId] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblTimeline]  WITH CHECK ADD  CONSTRAINT [FK_tblTimelines_tblMessages] FOREIGN KEY([MessageId])
REFERENCES [dbo].[tblMessage] ([Id])
GO

ALTER TABLE [dbo].[tblTimeline] CHECK CONSTRAINT [FK_tblTimelines_tblMessages]
GO


--
--
--
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblComment](
	[CommentToId] [int] NOT NULL,
	[MessageId] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblComment]  WITH CHECK ADD  CONSTRAINT [FK_tblComments_tblMessages_FKCommentToId] FOREIGN KEY([CommentToId])
REFERENCES [dbo].[tblMessage] ([Id])
GO

ALTER TABLE [dbo].[tblComment] CHECK CONSTRAINT [FK_tblComments_tblMessages_FKCommentToId]
GO

ALTER TABLE [dbo].[tblComment]  WITH CHECK ADD  CONSTRAINT [FK_tblComments_tblMessages_FKMessageId] FOREIGN KEY([MessageId])
REFERENCES [dbo].[tblMessage] ([Id])
GO

ALTER TABLE [dbo].[tblComment] CHECK CONSTRAINT [FK_tblComments_tblMessages_FKMessageId]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblConferenceMembers](
	[ConferenceID] [int] NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]

GO


