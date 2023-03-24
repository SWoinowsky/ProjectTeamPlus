using SteamProject.Models;
using SteamProject.ViewModels;
using System.Text.Json;
using SteamProject.Models.DTO;
using NUnit_Tests.RepoTesting;

namespace NUnit_Tests.ModelTesting
{
    public class FriendsPageVMTests
    {
        private List<Friend> _friends;
        private int _id;
        private string _steamId;

        [SetUp]
        public void Setup()
        {
            _id = 0;
            _steamId = "76561199486438115";
            _friends = new List<Friend>
                {
                    new Friend { Id = 1, RootId = 1, SteamId = "76561198952266051" },
                    new Friend { Id = 2, RootId = 2, SteamId = "76561198191586681" },
                    new Friend { Id = 3, RootId = 2, SteamId = "76561198166005873" },
                };
        }

        private FriendsPageVM MakeValidFriendsPageVM()
        {
            FriendsPageVM friendsPageVM = new FriendsPageVM
            (
                friends: _friends,
                id: _id,
                steamId: _steamId
            );

            return friendsPageVM;
        }

        [Test]
        public void FriendsPageVM_HasFriends_VM_isValid()
        {
            // Arrange
            FriendsPageVM friendsPageVM = MakeValidFriendsPageVM();

            // Act
            ModelValidator mv = new ModelValidator(friendsPageVM);

            // Assert
            Assert.That(mv.Valid, Is.True);
        }

        [Test]
        public void FriendsPageVM_HasNoFriends_VM_isStillValid()
        {
            // Arrange
            FriendsPageVM friendsPageVM = MakeValidFriendsPageVM();
            friendsPageVM.Friends = null;
            // Act
            ModelValidator mv = new ModelValidator(friendsPageVM);

            // Assert
            Assert.That(mv.Valid, Is.True);
        }
    }
}