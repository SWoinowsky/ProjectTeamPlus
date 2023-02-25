USE [SteamInfo];

ALTER TABLE [Friend] DROP CONSTRAINT [Friend_Fk_User];
ALTER TABLE [UserAchievement] DROP CONSTRAINT [UserAchievement_Fk_User];
ALTER TABLE [UserAchievement] DROP CONSTRAINT [UserAchievement_Fk_Achievement];
ALTER TABLE [UserGameInfo] DROP CONSTRAINT [UserGameInfo_FK_User];
ALTER TABLE [UserGameInfo] DROP CONSTRAINT [UserGameInfo_FK_Game];


DROP TABLE [User];
DROP TABLE [Friend];
DROP TABLE [Game];
DROP TABLE [GameAchievement];
DROP TABLE [UserAchievement];
DROP TABLE [UserGameInfo]