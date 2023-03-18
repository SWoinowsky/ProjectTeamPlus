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
using SteamProject.Models.DTO;

namespace NUnit_Tests.RepoTesting
{
    public class GameVMTests
    {
        private GameVM MakeValidGameVM()
        {
            GameVM gameVm = new GameVM{
                _appId = 1,
                _userGame = new UserGameInfo(),
                _game = new Game{
                    Id = 1,
                    AppId = 1,
                    Name = "A Game"
                },
                _poco = new GameInfoPOCO(),
                playTime = 100
            };
            return gameVm;
        }
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GameVM_HasGame_isValid()
        {
            // Arrange
            GameVM gameVm = MakeValidGameVM();

            // Act
            ModelValidator mv = new ModelValidator(gameVm);

            // Assert
            Assert.That(mv.Valid, Is.True);
        }

        

    }
}