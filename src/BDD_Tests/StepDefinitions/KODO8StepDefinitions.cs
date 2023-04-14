using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;

namespace BDD_Tests.StepDefinitions
{

    [Binding]
    public class KODO8StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly HomePageObject _homePage;
        private readonly ProfilePageObject _profilePage;

        public KODO8StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _profilePage = new ProfilePageObject(browserDriver.Current);

            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO129StepDefinitions>();
            Configuration = builder.Build();
        }

        [Then(@"I can see my Steam profile image")]
        public void ThenICanSeeMySteamProfileImage()
        {
            _profilePage.ProfileImageVisible().Should().BeTrue();
        }

        [Then(@"I can see my Steam username and level")]
        public void ThenICanSeeMySteamUsernameAndLevel()
        {
            _profilePage.UsernameAndLevelVisible().Should().BeTrue();
        }
    }
}
