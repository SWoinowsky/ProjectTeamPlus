USE [SteamInfo];

ALTER TABLE [Friend] DROP CONSTRAINT [Friend_Fk_User];
ALTER TABLE [GameAchievement] DROP CONSTRAINT [GameAchievement_Fk_Game];
ALTER TABLE [UserAchievement] DROP CONSTRAINT [UserAchievement_Fk_Game];

DROP TABLE [User];
DROP TABLE [Friend];
DROP TABLE [Game];
DROP TABLE [GameAchievement];
DROP TABLE [UserAchievement];