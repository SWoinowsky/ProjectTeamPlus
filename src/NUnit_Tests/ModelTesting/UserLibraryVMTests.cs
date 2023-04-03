using NUnit_Tests.RepoTesting;
using SteamProject.Models;
using SteamProject.ViewModels;


namespace NUnit_Tests.ModelTesting
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
                _games = new HashSet<Game>()
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