using Microsoft.EntityFrameworkCore;
using Moq;
using SteamProject.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using System;

namespace NUnit_Tests.RepoTesting
{
    public class GameRepositoryTests
    {
        private Mock<SteamInfoDbContext> _mockContext;
        private Mock<DbSet<Game>> _mockGameDbSet;

        private List<User> _users;
        private List<Friend> _friends;
        private List<UserGameInfo> _userGameInfo;
        private List<Game> _games;

        private Game MakeValidGame()  
        {
            Game newGame = new Game
            {
                Id = 1,
                AppId = 310560,
                Name = "DiRT Rally",
            };
            return newGame;
        }

        [SetUp]
        public void Setup()
        {
            _users = new List<User>
            {
                new User {Id= 1, SteamName = "TestUser1", SteamId = "76561198280399190", AspNetUserId = "f7b530ec-bf89-45e1-8080-cbe3bfd0f08a" },
                new User {Id= 2, SteamName = "TestUser2", SteamId = "76561198078883932", AspNetUserId = "f7b530ec-bf89-45e1-8080-cbe3bfd0f08g"},
                new User {Id= 3, SteamName = "TestUser3", SteamId = "76561198368539189", AspNetUserId = "f7b530ec-bf89-45e1-8080-cbe3bfd0f08h"}
            };

            _friends = new List<Friend>
            {
                new Friend {Id = 1, RootId = 1, SteamId = "76561198078883932"},
                new Friend {Id = 2, RootId = 2, SteamId = "76561198280399190"},
                new Friend {Id = 3, RootId = 2, SteamId = "76561198368539189"},
            };

            _games = new List<Game>()
            {
                new Game {Id = 1,AppId = 310560, Name = "DiRT Rally"},
                new Game {Id = 2,AppId = 218620, Name = "PAYDAY 2"},
                new Game {Id = 3,AppId = 632360, Name = "Risk of Rain 2"},
            };

            _userGameInfo = new List<UserGameInfo>()
            {
                new UserGameInfo { Id = 1, Followed = true, GameId = 1, Hidden = false, OwnerId = 1, Game = _games[0]},
                new UserGameInfo { Id = 2, Followed = true, GameId = 2, Hidden = false, OwnerId = 1, Game = _games[1]},
                new UserGameInfo { Id = 3, Followed = true, GameId = 3, Hidden = true, OwnerId = 2, Game = _games[2]},
            };

            _users.ForEach(p =>
            {
                p.Friends = _friends.Where(i => i.Id == p.Id).ToList();
                p.UserGameInfos = _userGameInfo.Where(i => i.OwnerId == p.Id).ToList();
            });

            // Finally, mock the context and dbsets
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockGameDbSet = MockHelpers.GetMockDbSet(_games.AsQueryable());
            _mockContext.Setup(ctx => ctx.Games).Returns(_mockGameDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Game>()).Returns(_mockGameDbSet.Object);
        }

        [Test]
        public void GetGameByAppId_WithThreeItems_Returns_True()
        {
            // Arrange
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            Game expected = MakeValidGame();


            // Act
            Game actual = gameRepository.GetGameByAppId(310560);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(actual.AppId == expected.AppId);
            });
        }

        [Test]
        public void GetGameByAppId_WithWrongGame_Returns_True()
        {
            // Arrange
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            Game expected = MakeValidGame();


            // Act
            Game actual = gameRepository.GetGameByAppId(310565);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(actual == null);
            });
        }
        [Test]
        public void GetGameByAppId_WithNoItems_Returns_Null()
        {
            // Arrange

            _users.Clear();
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockGameDbSet = MockHelpers.GetMockDbSet(_games.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Game>()).Returns(_mockGameDbSet.Object);
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);


            // Act
            Game actual = gameRepository.GetGameByAppId(310565);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(actual == null);
            });
        }
        [Test]
        public void GetGamesList_WithNoItems_Returns_Null()
        {
            // Arrange

            _users.Clear();
            _userGameInfo.Clear();
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockGameDbSet = MockHelpers.GetMockDbSet(_games.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Game>()).Returns(_mockGameDbSet.Object);
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);


            // Act
            HashSet<Game> actual = gameRepository.GetGamesListByUserInfo(_userGameInfo);


            Assert.Multiple(() =>
            {
                Assert.True(actual.Count == 0);
            });
        }
        [Test]
        public void GetGamesList_WithThreeItems_Returns_True()
        {
            // Arrange

            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            Game expected = MakeValidGame();

            // Act
            HashSet<Game> actual = gameRepository.GetGamesListByUserInfo(_userGameInfo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(actual.Count == 3);
            });
        }

        [Test]
        public void GetGamesListByUserInfo_WithEmptyUserGameInfoList_Returns_EmptyList()
        {
            // Arrange
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<UserGameInfo> emptyUserGameInfoList = new List<UserGameInfo>();

            // Act
            HashSet<Game> actual = gameRepository.GetGamesListByUserInfo(emptyUserGameInfoList);

            // Assert
            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetGamesListByUserInfo_WithDuplicateGameIds_Returns_UniqueGamesList()
        {
            // Arrange
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            _userGameInfo.Add(new UserGameInfo { Id = 4, Followed = true, GameId = 3, Hidden = false, OwnerId = 1 });
            int expectedCount = 3;

            // Act
            HashSet<Game> actual = gameRepository.GetGamesListByUserInfo(_userGameInfo);

            // Assert
            Assert.AreEqual(expectedCount, actual.Count);
        }

    }
}
