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
        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly FriendsPageObject _friendsPage;
        private readonly LoginPageObject _loginPage;
        

        public KODO175StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _friendsPage = new FriendsPageObject(browserDriver.Current);
            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO175StepDefinitions>();
            Configuration = builder.Build();
        }


        [Given(@"I have a friend on the friends page")]
        public void GivenIHaveAFriendOnTheFriendsPage()
        {
            //_friendsPage.LoadAllCookies();
            Thread.Sleep(500);
            _friendsPage.GoTo("Friends");
            Thread.Sleep(500);
            TestUser user = new TestUser
            {
                UserName = "TestUser",
                FirstName = "The",
                LastName = "Admin",
                Email = "TestUser@mail.com",
                Password = Configuration["SeedUserPW"]
            };
            Thread.Sleep(500);
            _loginPage.EnterEmail(user.Email);
            _loginPage.EnterPassword(user.Password);
            Thread.Sleep(500);
            _loginPage.Login();
            Thread.Sleep(500);
            _friendsPage.GoTo("Friends");

        }

        [Given(@"I click on the name on the friend card")]
        public void GivenIClickOnTheNameOnTheFriendCard()
        {
            Thread.Sleep(500);
            _friendsPage.ClickName();
        }

        [When(@"I enter a nickname ""([^""]*)""")]
        public void WhenIEnterANickname(string alias)
        {
            Thread.Sleep(500);
            _friendsPage.GiveNewName("Minecraft");
        }

        [Then(@"I should see them references as ""([^""]*)""")]
        public void ThenIShouldSeeThemReferencesAs(string alias)
        {
            Thread.Sleep(500);
            _friendsPage.NewName().Should().BeEquivalentTo("Minecraft");
        }
    }
}
