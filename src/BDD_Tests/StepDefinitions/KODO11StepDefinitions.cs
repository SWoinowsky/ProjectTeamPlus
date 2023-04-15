using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using BDD_Tests.Drivers;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO11StepDefinitions
    {
        private readonly ExternalLoginsPageObject _externalLoginsPage;

        public KODO11StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _externalLoginsPage = new ExternalLoginsPageObject(browserDriver.Current);

        }

        [When(@"I click on the Remove Steam link button")]
        public void WhenIClickOnTheRemoveSteamLinkButton()
        {
            _externalLoginsPage.SteamUnLinkButtonClick();
        }

        [Then(@"I should see the button to link a steam account")]
        public void ThenIShouldSeeTheButtonToLinkASteamAccount()
        {
            Thread.Sleep(1000);
            _externalLoginsPage.SteamLinkButtonIsVisible().Should().BeTrue();
        }
    }
}
