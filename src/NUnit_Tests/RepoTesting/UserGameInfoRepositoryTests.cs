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
    public class UserGameInfoRepositoryTests
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
                new Game { Id = 3, AppId = 632360, Name = "Risk of Rain 2" },
            };
            _userGameInfos = new List<UserGameInfo>()
            {
                new UserGameInfo { Id = 1, Followed = true, GameId = 1, Hidden = false, OwnerId = 1, Game = _games[0] },
                new UserGameInfo { Id = 2, Followed = true, GameId = 2, Hidden = false, OwnerId = 2, Game = _games[1] },
                new UserGameInfo { Id = 3, Followed = true, GameId = 3, Hidden = true, OwnerId = 2, Game = _games[2] },
            };
           


            _users.ForEach(p =>
            {
                p.Friends = _friends.Where(i => i.RootId == p.Id).ToList() ?? new List<Friend>();
                p.UserGameInfos = _userGameInfos.Where(i => i.OwnerId == p.Id).ToList() ?? new List<UserGameInfo>();
            });



            // Finally, mock the context and dbsets
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockGameInfoDbSet = MockHelpers.GetMockDbSet(_userGameInfos.AsQueryable());
            _mockContext.Setup(ctx => ctx.UserGameInfos).Returns(_mockGameInfoDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<UserGameInfo>()).Returns(_mockGameInfoDbSet.Object);
        }

        [Test]
        public void GetUserInfoForGame_WithThreeItems_Returns_True()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);
            UserGameInfo expected = MakeValidGameInfo();


            // Act
            UserGameInfo? actual = gameInfoRepository.GetUserInfoForGame(310560, 1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(actual.GameId == expected.GameId);
                Assert.True(actual.OwnerId == expected.OwnerId);
            });
        }

        [Test]
        public void GetUserInfoForGame_WithWrongGame_Returns_True()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);
            UserGameInfo expected = MakeValidGameInfo();

            // Act
            UserGameInfo? actual = gameInfoRepository.GetUserInfoForGame(66666, 1);

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void GetUserInfoForGame_WithNoItems_Returns_Null()
        {
            // Arrange

            _userGameInfos.Clear();
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockGameInfoDbSet = MockHelpers.GetMockDbSet(_userGameInfos.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<UserGameInfo>()).Returns(_mockGameInfoDbSet.Object);
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);


            // Act
            UserGameInfo actual = gameInfoRepository.GetUserInfoForGame(310560, 2);

            // Assert
            Assert.Multiple(() => { Assert.True(actual == null); });
        }

        [Test]
        public void GetAllUserGameInfo_WithNoItems_Returns_Null()
        {
            // Arrange
            _users.Clear();
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockGameInfoDbSet = MockHelpers.GetMockDbSet(_userGameInfos.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<UserGameInfo>()).Returns(_mockGameInfoDbSet.Object);
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);


            // Act
            List<UserGameInfo> actual = gameInfoRepository.GetAllUserGameInfo(3);

            // Assert
            Assert.Multiple(() => { Assert.True(actual.Count == 0); });
        }

        [Test]
        public void GetGamesList_WithTwoItems_Returns_True()
        {
            // Arrange
            IUserGameInfoRepository gameInfoRepository = new UserGameInfoRepository(_mockContext.Object);


            // Act
            List<UserGameInfo> actual = gameInfoRepository.GetAllUserGameInfo(2);

            // Assert
            Assert.Multiple(() => { Assert.True(actual.Count == 2); });

        }
    }
}