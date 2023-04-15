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

        private readonly HomePageObject _homePage;
        private readonly DashboardPageObject _dashboardPage;
        private readonly LibraryPageObject _libraryPage;


        public KODO30StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _dashboardPage = new DashboardPageObject(browserDriver.Current);
            _libraryPage = new LibraryPageObject(browserDriver.Current);

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO30StepDefinitions>();
            Configuration = builder.Build();
        }
        [When(@"I click on the dashboard link")]
        public void WhenIClickOnTheDashboardLink()
        {
            _homePage.GoTo("Dashboard");
        }

        [Then(@"I end up on the dashboard page")]
        public void ThenIEndUpOnTheDashboardPage()
        {
            var title = _dashboardPage.GetTitle();
            _dashboardPage.GetTitle().Should().ContainEquivalentOf("Dashboard", AtLeast.Once());
        }

        [Then(@"I can see my Steam information displayed on the dashboard")]
        public void ThenICanSeeMySteamInformationDisplayedOnTheDashboard()
        {
            _dashboardPage.ProfileImageVisible().Should().BeTrue();
            _dashboardPage.UsernameAndLevelVisible().Should().BeTrue();
        }

        [Then(@"I should see my recent games carousel")]
        public void ThenIShouldSeeMyRecentGamesCarousel()
        {
            _dashboardPage.RecentGamesIsVisible().Should().BeTrue();
        }

        [Then(@"I should not see my followed games carousel")]
        public void ThenIShouldNotSeeMyFollowedGamesCarousel()
        {
            _dashboardPage.FollowedGamesIsVisible().Should().BeFalse();
        }

        [When(@"I click on the library link")]
        [Then(@"I click on the library link")]
        public void WhenIClickOnTheLibraryLink()
        {
            _homePage.GoTo("Library");
        }

        [When(@"I should see and be able to follow a game")]
        public void WhenIShouldSeeAndBeAbleToFollowAGame()
        {
            _libraryPage.FollowFirstGame();
        }

        [Then(@"I should see my followed games carousel")]
        public void ThenIShouldSeeMyFollowedGamesCarousel()
        {
            _dashboardPage.FollowedGamesIsVisible().Should().BeTrue();
        }

        [Then(@"I should see and be able to unFollow that same game")]
        public void ThenIShouldSeeAndBeAbleToUnFollowThatSameGame()
        {
            _libraryPage.FollowFirstGame();
        }


    }
}
