using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using BDD_Tests.Drivers;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO30StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPageObject _loginPage;
        private readonly HomePageObject _homePage;
        private readonly DashboardPageObject _dashboardPage;
        private readonly CompetePageObject _competePage;

        public KODO30StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _homePage = new HomePageObject(browserDriver.Current);
            _competePage = new CompetePageObject(browserDriver.Current);
            _dashboardPage = new DashboardPageObject(browserDriver.Current);
            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO129StepDefinitions>();
            Configuration = builder.Build();
        }
        [When(@"I click on the dashboard link")]
        public void WhenIClickOnTheDashboardLink()
        {
            _homePage.ClickNavBarDashboardLink(); // it only works if you tell it twice, code moment
            Thread.Sleep(1000);
            _homePage.ClickNavBarDashboardLink();
            Thread.Sleep(1000);
        }

        [Then(@"I end up on the dashboard page")]
        public void ThenIEndUpOnTheDashboardPage()
        {
            var title = _competePage.GetTitle();
            _dashboardPage.GetTitle().Should().ContainEquivalentOf("Dashboard", AtLeast.Once());
        }

        [Then(@"I can see my Steam information displayed on the dashboard")]
        public void ThenICanSeeMySteamInformationDisplayedOnTheDashboard()
        {
            _dashboardPage.ProfileImageVisible().Should().BeTrue();
            _dashboardPage.UsernameAndLevelVisible().Should().BeTrue();
        }

    }
}
