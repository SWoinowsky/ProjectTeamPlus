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

        [When(@"I click on Eithn�'s friend page link")]
        public void WhenIClickOnEithn�sFriendPageLink()
        {
            _profilePage.Eithn�PageClick();
        }


        [Then(@"I can see the shared games page for Eithn�")]
        public void ThenISeeSharedGamesForEithn�()
        {
            var title = _friendPage.GetTitle();
            title.Should().ContainEquivalentOf("Friend", AtLeast.Once());
        }

        [Then(@"I can see Eithn�'s username")]
        public void ThenICanSeeEithn�Username()
        {
            _friendPage.GetFriendUsername().GetAttribute("innerHTML").Should().ContainEquivalentOf("Eithn�", AtLeast.Once());
        }

    }
}