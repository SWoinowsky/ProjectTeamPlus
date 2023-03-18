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
using SteamProject.ViewModels;
using System.Text.Json;
using SteamProject.Models.DTO;

namespace NUnit_Tests.RepoTesting
{
    public class UserLibraryVMTests
    {
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
        private UserLibraryVM MakeValidUserLibraryVM()
        {
            UserLibraryVM vm = new UserLibraryVM()
            {
                _user = MakeValidPerson(),
                _games = new List<Game>()
            };
            return vm;
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UserLibraryVM_isValid()
        {
            // Assert
            UserLibraryVM vm = new UserLibraryVM();

            // Act
            vm = MakeValidUserLibraryVM();
            ModelValidator mv = new ModelValidator(vm);

            // Assert
            Assert.That(mv.Valid, Is.True);
        }
    }
}