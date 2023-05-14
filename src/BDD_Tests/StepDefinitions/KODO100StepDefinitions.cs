using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;

namespace BDD_Tests.StepDefinitions
{

    [Binding]
    public class KODO100StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly HomePageObject _homePage;
        private readonly ProfilePageObject _profilePage;
        private readonly FriendPageObject _friendPage;

        public KODO100StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _profilePage = new ProfilePageObject(browserDriver.Current);
            _friendPage = new FriendPageObject(browserDriver.Current);


            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO129StepDefinitions>();
            Configuration = builder.Build();
        }

        [When(@"I click on ""(.*)'s"" friend page link")]
        public void WhenIClickOnFriendsPageLink(string friendName)
        {
            _profilePage.GoToFriendPage(friendName);
        }

        [Then(@"I can see the shared games page for ""(.*)""")]
        public void ThenISeeSharedGamesForFriend(string friendName)
        {   
            Thread.Sleep(500);
            var sharedGames = _friendPage.GetSharedGames();
            sharedGames.Should().NotBeNullOrEmpty("No shared games found for friend: " + friendName);
        }


        [Then(@"I can see ""(.*)'s"" username")]
        public void ThenICanSeeFriendUsername(string friendName)
        {
            _friendPage.GetFriendUsername().GetAttribute("innerHTML").Should().ContainEquivalentOf(friendName, AtLeast.Once());
        }


    }
}
