using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO138StepDefinitions
    {

        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly FriendsPageObject _friendsPage;
        private readonly LoginPageObject _loginPage;


        public KODO138StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _friendsPage = new FriendsPageObject(browserDriver.Current);
            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO138StepDefinitions>();
            Configuration = builder.Build();
        }

        [When(@"I enter ""([^""]*)"" in the search box")]
        public void WhenIEnterInTheSearchBox(string name)
        {
           
            _friendsPage.FriendSearch.Click();
            Thread.Sleep(500);
            _friendsPage.FriendSearch.SendKeys(name);
        }

        [Then(@"I should see their friend card on the page")]
        public void ThenIShouldSeeTheirFriendCardOnThePage()
        {
            Thread.Sleep(500);
            _friendsPage.FriendNamedSteve.Displayed.Should().BeTrue();
        }
    }
}
