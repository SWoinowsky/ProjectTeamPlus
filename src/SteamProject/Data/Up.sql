-- Highlight then ctrl+k, ctrl+c to uncomment / recomment

--CREATE DATABASE [SteamInfo];

USE [SteamInfo];

CREATE TABLE [User]
(
	[Id]				INT				NOT NULL IDENTITY(1,1) PRIMARY KEY
	,[AspNetUserId]		NVARCHAR(450)	NOT NULL
	,[SteamId]			NVARCHAR(50)
	,[SteamName]		NVARCHAR(50)
	,[ProfileUrl]		NVARCHAR(100)
	,[AvatarUrl]		NVARCHAR(100)
	,[PersonaState]		INT
	,[PlayerLevel]		INT
);

CREATE TABLE [Friend]
(
	[Id]				INT				NOT NULL IDENTITY(1,1) PRIMARY KEY
	,[RootId]			INT				NOT NULL
	,[SteamId]			NVARCHAR(50)	NOT NULL
	,[SteamName]		NVARCHAR(50)	NOT NULL
	,[PersonaState]		INT				NOT NULL
	,[AvatarUrl]		NVARCHAR(100)	NOT NULL
	,[AvatarFullUrl]	NVARCHAR(100)	NOT NULL
	,[LastLogOff]		INT				NOT NULL
	,[GameExtraInfo]	NVARCHAR(100)
	,[GameId]			NVARCHAR(100)
);

CREATE TABLE [Game]
(
	[Id]				INT				NOT NULL IDENTITY(1,1) PRIMARY KEY
	,[OwnerId]			INT				NOT NULL
	,[AppId]			INT				NOT NULL
	,[Name]				NVARCHAR(512)	NOT NULL
	,[DescShort]		NVARCHAR(512)	NOT NULL
	,[DescLong]			NVARCHAR(1024)	NOT NULL
	,[PlayTime]			INT				NOT NULL
	,[IconUrl]			NVARCHAR(512)	NOT NULL
	,[LastPlayed]		INT				NOT NULL
	,[Hidden]			BIT				NOT NULL
);

CREATE TABLE [GameAchievement]
(
	[Id]				INT				NOT NULL IDENTITY(1,1) PRIMARY KEY
	,[GameId]			INT				NOT NULL
	,[ApiName]			NVARCHAR(100)	NOT NULL
	,[DisplayName]		NVARCHAR(50)	NOT NULL
	,[IconAchievedUrl]	NVARCHAR(100)	NOT NULL
	,[IconHiddenUrl]	NVARCHAR(100)	NOT NULL
	,[HiddenFromUsers]	BIT				NOT NULL
);

CREATE TABLE [UserAchievement]
(
	[Id]				INT				NOT NULL IDENTITY(1,1) PRIMARY KEY
	,[OwnerId]			INT				NOT NULL
	,[GameId]			INT				NOT NULL
	,[ApiName]			NVARCHAR(100)	NOT NULL
	,[DisplayName]		NVARCHAR(50)	NOT NULL
	,[Achieved]			BIT				NOT NULL
	,[UnlockTime]		INT				
);

ALTER TABLE [Friend] ADD CONSTRAINT [Friend_Fk_User] FOREIGN KEY ([RootId]) REFERENCES [User] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [Game] ADD CONSTRAINT [Game_Fk_User] FOREIGN KEY ([OwnerId]) REFERENCES [User] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [UserAchievement] ADD CONSTRAINT [UserAchievement_Fk_User] FOREIGN KEY ([OwnerId]) REFERENCES [User] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


