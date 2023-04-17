using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;
namespace BDD_Tests.StepDefinitions

{
    [Binding]
    public class KODO175StepDefinitions_2
    {

        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly FriendsPageObject _friendsPage;
        private readonly LoginPageObject _loginPage;


        public KODO175StepDefinitions_2(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _friendsPage = new FriendsPageObject(browserDriver.Current);
            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO175StepDefinitions_2>();
            Configuration = builder.Build();
        }

        [Given(@"That friend has a set nickname")]
        public void GivenThatFriendHasASetNickname()
        {
            Thread.Sleep(500);
            _friendsPage.CardName.Text.Should().BeEquivalentTo("Minecraft");
        }

        [When(@"I click on the revert button")]
        public void WhenIClickOnTheRevertButton()
        {
            Thread.Sleep(500);
            _friendsPage.Revert.Click();
        }

        [Then(@"I should see their name return to original")]
        public void ThenIShouldSeeTheirNameReturnToOriginal()
        {
            Thread.Sleep(1000);
            _friendsPage.CardName.Text.Should().BeEquivalentTo("Steve");
        }
    }
}
