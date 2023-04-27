USE [SteamInfo];

ALTER TABLE [Friend]			            DROP CONSTRAINT [Friend_Fk_User];
ALTER TABLE [UserAchievement]	            DROP CONSTRAINT [UserAchievement_Fk_User];
ALTER TABLE [UserAchievement]	            DROP CONSTRAINT [UserAchievement_Fk_Achievement];
ALTER TABLE [UserGameInfo]		            DROP CONSTRAINT [UserGameInfo_FK_User];
ALTER TABLE [UserGameInfo]		            DROP CONSTRAINT [UserGameInfo_FK_Game];
ALTER TABLE [Competition]		            DROP CONSTRAINT [Competition_Fk_Game];
ALTER TABLE [CompetitionPlayer]		        DROP CONSTRAINT [CompetitionPlayer_Fk_Competition];
ALTER TABLE [CompetitionGameAchievement]	DROP CONSTRAINT [CompetitionGameAchievement_Fk_Competition];
ALTER TABLE [InboxMessage]                  DROP CONSTRAINT [InboxMessage_Fk_User];

DROP TABLE [User];
DROP TABLE [Friend];
DROP TABLE [Game];
DROP TABLE [GameAchievement];
DROP TABLE [UserAchievement];
DROP TABLE [UserGameInfo];
DROP TABLE [Competition];
DROP TABLE [CompetitionPlayer];
DROP TABLE [CompetitionGameAchievement];
DROP TABLE [BlackList];
DROP TABLE [AdminUser];
DROP TABLE [InboxMessage];