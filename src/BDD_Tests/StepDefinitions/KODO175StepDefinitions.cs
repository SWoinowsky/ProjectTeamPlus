using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO175StepDefinitions
    {
        //private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        //private readonly LoginPageObject _loginPage;
        //private readonly HomePageObject _homePage;
        //private readonly CompetePageObject _competePage;
        private readonly FriendsPageObject _friendsPage;

        public KODO175StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            //_loginPage = new LoginPageObject(browserDriver.Current);
            //_homePage = new HomePageObject(browserDriver.Current);
            //_competePage = new CompetePageObject(browserDriver.Current);
            _friendsPage = new FriendsPageObject(browserDriver.Current);
            _scenarioContext = context;

            //IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO129StepDefinitions>();
            //Configuration = builder.Build();
        }

        [Given(@"I have a friend on the friends page")]
        public void GivenIHaveAFriendOnTheFriendsPage()
        {
            //_friendsPage.SaveAllCookies();
            _friendsPage.LoadAllCookies();
            _friendsPage.GoTo("Home");
        }

        [Given(@"I click on the name on the friend card")]
        public void GivenIClickOnTheNameOnTheFriendCard()
        {
            _friendsPage.ClickName();
        }

        [When(@"I enter a nickname ""([^""]*)""")]
        public void WhenIEnterANickname(string alias)
        {
            _friendsPage.GiveNewName();
        }

        [Then(@"I should see them references as ""([^""]*)""")]
        public void ThenIShouldSeeThemReferencesAs(string alias)
        {
            _friendsPage.NewName().Should().BeEquivalentTo("Minecraft");
        }
    }
}
