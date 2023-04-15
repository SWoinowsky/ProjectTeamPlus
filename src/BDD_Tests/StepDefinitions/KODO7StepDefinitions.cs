using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;

namespace BDD_Tests.StepDefinitions
{

    [Binding]
    public class KODO7StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly HomePageObject _homePage;
        private readonly ProfilePageObject _profilePage;

        public KODO7StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _profilePage = new ProfilePageObject(browserDriver.Current);

            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO129StepDefinitions>();
            Configuration = builder.Build();
        }


        [When(@"I click on the profile link")]
        [Given(@"I click on the profile link")]
        public void WhenIClickOnTheProfileLink()
        {
            _homePage.ClickNavBarProfileLink(); // it only works if you tell it twice, code moment
            Thread.Sleep(1000);
            _homePage.ClickNavBarProfileLink();
            Thread.Sleep(1000);
        }

        [Then(@"I can see my list of friends")]
        public void ThenICanSeeMyListOfFriends()
        {
            _profilePage.FriendsListVisible().Should().BeTrue();
        }

    }
}
