USE [SteamInfo];

ALTER TABLE [Friend] DROP CONSTRAINT [Friend_Fk_User];
ALTER TABLE [Game] DROP CONSTRAINT [Game_Fk_User];
ALTER TABLE [UserAchievement] DROP CONSTRAINT [UserAchievement_Fk_User];

DROP TABLE [User];
DROP TABLE [Friend];
DROP TABLE [Game];
DROP TABLE [GameAchievement];
DROP TABLE [UserAchievement];