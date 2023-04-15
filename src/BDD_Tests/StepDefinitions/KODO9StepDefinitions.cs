using BDD_Tests.PageObjects;
using System;
using BDD_Tests.Drivers;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO9Definitions
    {
        private readonly ProfilePageObject _profilePage;
        private readonly ExternalLoginsPageObject _externalLoginsPage;
        private readonly SteamLoginPageObject _steamLoginPage;
        private readonly LibraryPageObject _libraryPage;

        public KODO9Definitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _profilePage = new ProfilePageObject(browserDriver.Current);
            _externalLoginsPage = new ExternalLoginsPageObject(browserDriver.Current);
            _steamLoginPage = new SteamLoginPageObject(browserDriver.Current);
            _libraryPage = new LibraryPageObject(browserDriver.Current);
        }

        [When(@"I click on the Steam Account link")]
        [Given(@"I click on the Steam Account link")]
        public void WhenIClickOnTheSteamAccountLink()
        {
            _profilePage.SteamLinkPageClick();
        }

        [When(@"I click on the Steam link button")]
        [Then(@"I click on the Steam link button")]
        public void WhenIClickOnTheSteamLinkButton()
        {
            _externalLoginsPage.SteamLinkButtonClick();
        }

        [When(@"I am redirected to steams login page")]
        [Then(@"I should be redirected to steams login page")]
        public void ThenIShouldBeRedirectedToSteamsLoginPage()
        {
            _steamLoginPage.LoadAllCookies().Should().BeTrue();
        }

        [When(@"I am able to click sign in")]
        [Then(@"I should be able to click sign in")]
        public void ThenIShouldBeAbleToClickSignIn()
        {
            _steamLoginPage.SteamLinkSignInButton.Click();
        }

        [When(@"I am redirected back and see my library")]
        [Then(@"I should be redirected back and see my library")]
        public void ThenThenIShouldBeRedirectedBackAndSeeMyLibrary()
        {
            _libraryPage.SteamAvatarImgVisible().Should().BeTrue();
        }

        [Then(@"I should see my SteamId displayed")]
        public void ThenIShouldSeeMySteamIdDisplayed()
        {
            _externalLoginsPage.SteamIdIsVisible().Should().BeTrue();
        }



    }
}
