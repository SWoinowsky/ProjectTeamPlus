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
    public class UserRepositoryTests
    {
        private Mock<SteamInfoDbContext> _mockContext;
        private Mock<DbSet<User>> _mockPersonDbSet;

        private List<User> _users;
        private List<Friend> _friends;
        private List<UserGameInfo> _userGameInfo;
        private List<Game> _game;

        private User MakeValidPerson()
        {
            User newPerson = new User
            {
                Id = 1,
                SteamId = "76561198053115178",
                SteamName = "TestUser1",
                Friends = new List<Friend>(),
                UserAchievements = new List<UserAchievement>(),
                UserGameInfos = new List<UserGameInfo>(),
                AspNetUserId = "f7b530ec-bf89-45e1-8080-cbe3bfd0f08a"
            };
            return newPerson;
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
            

            _userGameInfo = new List<UserGameInfo>()
            {
                new UserGameInfo { Id = 1, Followed = true, GameId = 1, Hidden = false, OwnerId = 1},
                new UserGameInfo { Id = 2, Followed = true, GameId = 2, Hidden = false, OwnerId = 1},
                new UserGameInfo { Id = 3, Followed = true, GameId = 2, Hidden = true, OwnerId = 1},
            };
            _game = new List<Game>()
            {
                new Game {Id = 1,AppId = 310560, Name = "DiRT Rally"},
                new Game {Id = 2,AppId = 218620, Name = "PAYDAY 2"},
                new Game {Id = 2,AppId = 632360, Name = "Risk of Rain 2"},
            };


            _users.ForEach(p =>
            {
                p.Friends = _friends.Where(i => i.Id == p.Id).ToList();
                p.UserGameInfos = _userGameInfo.Where(i => i.Id == p.Id).ToList();
            });


            // Finally, mock the context and dbsets
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockPersonDbSet = MockHelpers.GetMockDbSet(_users.AsQueryable());
            _mockContext.Setup(ctx => ctx.Users).Returns(_mockPersonDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<User>()).Returns(_mockPersonDbSet.Object);
        }

        [Test]
        public void GetUserById_WithThreeItems_ReturnsTrue()
        {
            // Arrange
            IUserRepository userRepository = new UserRepository(_mockContext.Object);
            User expected = MakeValidPerson();


            // Act
            User actual = userRepository.GetUser("f7b530ec-bf89-45e1-8080-cbe3bfd0f08a");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(actual.SteamName == expected.SteamName);
            });
        }

        [Test]
        public void GetUserById_WithWrongUser_ReturnsFalse()
        {
            // Arrange
            IUserRepository userRepository = new UserRepository(_mockContext.Object);
            User expected = MakeValidPerson();


            // Act
            User actual = userRepository.GetUser("f7b530ec-bf89-45e1-8080-cbe3bfd0f08g");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.False(actual.SteamName == expected.SteamName);
            });
        }
        [Test]
        public void GetUserById_WithNoItems_ReturnsTrue_ThrowsNull()
        {
            // Arrange

            _users.Clear();
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockPersonDbSet = MockHelpers.GetMockDbSet(_users.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<User>()).Returns(_mockPersonDbSet.Object);
            IUserRepository userRepository = new UserRepository(_mockContext.Object);

            // Act
            

            // Assert
            Assert.IsNull(userRepository.GetUser("f7b530ec-bf89-45e1-8080-cbe3bfd0f08a"));
        }

    }
}
