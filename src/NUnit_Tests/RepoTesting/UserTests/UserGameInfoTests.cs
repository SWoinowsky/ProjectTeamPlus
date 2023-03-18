using Microsoft.EntityFrameworkCore;
using Moq;
using SteamProject.Models;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;

namespace NUnit_Tests.RepoTesting
{
    public class UserGameInfoTests
    {
        private Mock<SteamInfoDbContext> _mockContext;
        private Mock<DbSet<UserGameInfo>> _mockGameInfoDbSet;

        private List<User> _users;
        private List<Friend> _friends;
        private List<UserGameInfo> _userGameInfos;
        private List<Game> _games;

        private UserGameInfo MakeValidGameInfo()
        {
            UserGameInfo newUserInfo = new UserGameInfo()
            {
                Id = 1,
                OwnerId = 1,
                GameId = 1,
                Hidden = false,
                Followed = false,
            };
            return newUserInfo;
        }

        [SetUp]
        public void Setup()
        {
            _users = new List<User>
            {
                new User
                {
                    Id = 1, SteamName = "TestUser1", SteamId = "76561198280399190",
                    AspNetUserId = "f7b530ec-bf89-45e1-8080-cbe3bfd0f08a"
                },
                new User
                {
                    Id = 2, SteamName = "TestUser2", SteamId = "76561198078883932",
                    AspNetUserId = "f7b530ec-bf89-45e1-8080-cbe3bfd0f08g"
                },
                new User
                {
                    Id = 3, SteamName = "TestUser3", SteamId = "76561198368539189",
                    AspNetUserId = "f7b530ec-bf89-45e1-8080-cbe3bfd0f08h"
                }

            };
            _friends = new List<Friend>
            {
                new Friend { Id = 1, RootId = 1, SteamId = "76561198078883932" },
                new Friend { Id = 2, RootId = 2, SteamId = "76561198280399190" },
                new Friend { Id = 3, RootId = 2, SteamId = "76561198368539189" },
            };

            _games = new List<Game>()
            {
                new Game { Id = 1, AppId = 310560, Name = "DiRT Rally" },
                new Game { Id = 2, AppId = 218620, Name = "PAYDAY 2" },
                new Game { Id = 2, AppId = 632360, Name = "Risk of Rain 2" },
            };

            _userGameInfos = new List<UserGameInfo>()
            {
                new UserGameInfo { Id = 1, Followed = true, GameId = 1, Hidden = false, OwnerId = 1, Game = _games[0] },
                new UserGameInfo { Id = 2, Followed = true, GameId = 2, Hidden = false, OwnerId = 1, Game = _games[1] },
                new UserGameInfo { Id = 3, Followed = true, GameId = 3, Hidden = true, OwnerId = 2, Game = _games[2] },
            };
            

            _users.ForEach(p =>
            {
                p.Friends = _friends.Where(i => i.Id == p.Id).ToList();
                p.UserGameInfos = _userGameInfos.Where(i => i.Id == p.Id).ToList();
            });

            // Finally, mock the context and dbsets
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockGameInfoDbSet = MockHelpers.GetMockDbSet(_userGameInfos.AsQueryable());
            _mockContext.Setup(ctx => ctx.UserGameInfos).Returns(_mockGameInfoDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<UserGameInfo>()).Returns(_mockGameInfoDbSet.Object);
        }

        [Test]
        public void SetHiddenStatusTrue_WithFalseHiddenSet_SetsGameHiddenStatusToTrue()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);
            List<UserGameInfo> gameList = gameInfoRepository.GetAllUserGameInfo(1);
            UserGameInfo game = new UserGameInfo();
            foreach(var temp in gameList)
            {
                if(temp.Id == 1 && temp.Hidden)
                {
                    game = temp;
                    break;
                }
            }

            // Act
            game.SetHiddenStatusTrue();

            // Assert
            Assert.True(game.Hidden);
        }

        [Test]
        public void SetHiddenStatusFalse_WithTrueHiddenSet_SetsGameHiddenStatusToFalse()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);
            List<UserGameInfo> gameList = gameInfoRepository.GetAllUserGameInfo(1);
            UserGameInfo game = new UserGameInfo();
            foreach(var temp in gameList)
            {
                if(temp.Id == 1 && !temp.Hidden)
                {
                    game = temp;
                    break;
                }
            }

            // Act
            game.SetHiddenStatusFalse();

            // Assert
            Assert.False(game.Hidden);
        }

        [Test]
        public void SetHiddenStatusFalse_WithFalseHiddenSet_WontChangeStatusFromFalse()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);
            List<UserGameInfo> gameList = gameInfoRepository.GetAllUserGameInfo(1);
            UserGameInfo game = new UserGameInfo();
            foreach(var temp in gameList)
            {
                if(temp.Id == 1 && !temp.Hidden)
                {
                    game = temp;
                    break;
                }
            }

            // Act
            game.SetHiddenStatusFalse();

            // Assert
            Assert.False(game.Hidden);
        }

        [Test]
        public void SetHiddenStatusTrue_WithTrueHiddenSet_WontChangeStatusFromTrue()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);
            List<UserGameInfo> gameList = gameInfoRepository.GetAllUserGameInfo(1);
            UserGameInfo game = new UserGameInfo();
            foreach(var temp in gameList)
            {
                if(temp.Id == 1 && temp.Hidden)
                {
                    game = temp;
                    break;
                }
            }

            // Act
            game.SetHiddenStatusTrue();

            // Assert
            Assert.True(game.Hidden);
        }

        [Test]
        public void GetGameByIdAndUser_WithValidGameIdAndUserId_ReturnsCorrectGame()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);
            UserGameInfo game = new UserGameInfo();
            UserGameInfo actual = new UserGameInfo 
            { 
                Id = 1, 
                Followed = true, 
                GameId = 1, 
                Hidden = false, 
                OwnerId = 1, 
                Game = _games[0]
                };

            // Act
            game = game.GetGameByIdAndUser(1, gameInfoRepository, 1);

            // Assert
            Assert.AreEqual(actual.Id, game.Id);
            Assert.AreEqual(actual.GameId, game.GameId);
            Assert.AreEqual(actual.Hidden, game.Hidden);
            Assert.AreEqual(actual.Followed, game.Followed);
            Assert.AreSame(actual.Game, game.Game);
        }

        [Test]
        public void GetGameByIdAndUser_WithInValidGameIdAndValidUserId_ReturnsNull()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);
            UserGameInfo game = new UserGameInfo();

            // Act
            game = game.GetGameByIdAndUser(10, gameInfoRepository, 1);

            // Assert
            Assert.IsNull(game);
        }
    }
}