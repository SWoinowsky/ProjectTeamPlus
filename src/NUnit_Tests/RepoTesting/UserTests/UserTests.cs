using SteamProject.Models;
using SteamProject.Models.DTO;
using System.Text.Json;

namespace NUnit_Tests.RepoTesting
{
    public class UserTests
    {
        private SteamUserPOCO _poco;
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
            _poco = JsonSerializer.Deserialize<SteamUserPOCO>("{\"response\":{\"players\":[{\"steamid\":\"1\",\"communityvisibilitystate\":1,\"profilestate\":1,\"personaname\":\"Justin Davis\",\"commentpermission\":1,\"profileurl\":\"bogusProfileURL\",\"avatar\":\"bogusAvatarjpg\",\"avatarmedium\":\"bogusMediumjpg\",\"avatarfull\":\"bogusFullAvatarjpg\",\"avatarhash\":\"badHash\",\"lastlogoff\":1111111111,\"personastate\":1,\"primaryclanid\":\"1\",\"timecreated\":1111111111,\"personastateflags\":1,\"loccountrycode\":\"US\"}]}}");
        }

        [Test]
        public void User_WithValidInputs_IsValid()
        {
            // Arrange
            User me = new User();

            // Act
            me = MakeValidPerson();
            ModelValidator mv = new ModelValidator(me);

            // Assert
            Assert.That(mv.Valid, Is.True);
        }

        [Test]
        public void TakeSteamPOCO_SetsPocoVariablesCorrectly ()
        {
            // Arrange
            User me = new User();
            me.Id = 1;
            me.AspNetUserId = "1";

            // Act
            me.TakeSteamPOCO(_poco);

            // Assert
            Assert.AreEqual(me.SteamId, "1");
            Assert.AreEqual(me.SteamName, "Justin Davis");
        }

                [Test]
        public void TakeSteamPOCO_InvalidPOCO_ThrowsNull ()
        {
            // Arrange
            User me = new User();
            me.Id = 1;
            me.AspNetUserId = "1";
            SteamUserPOCO bogusPoco = new SteamUserPOCO();

            // Act

            // Assert
            Assert.Throws<NullReferenceException>(() => me.TakeSteamPOCO(bogusPoco));
        }
    }
}
