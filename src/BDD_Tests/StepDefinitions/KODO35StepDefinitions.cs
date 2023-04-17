using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO35StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly HomePageObject _homePage;
        private readonly ScenarioContext _scenarioContext;
        private readonly LibraryPageObject _libraryPage;
        private readonly LoginPageObject _loginPage;

        public KODO35StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _scenarioContext = context;
            _homePage = new HomePageObject(browserDriver.Current);
            _libraryPage = new LibraryPageObject(browserDriver.Current);
            _loginPage = new LoginPageObject(browserDriver.Current);

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO30StepDefinitions>();
            Configuration = builder.Build();
        }

        [Then(@"I should see a button to hide ""([^""]*)""")]
        public void ThenIShouldSeeAButtonToHide(string p0)
        {
            _libraryPage.FindHideButtonForGame(p0);
        }

        [When(@"I click on the hide button for ""([^""]*)""")]
        public void WhenIClickOnTheHideButtonFor(string gameName)
        {
            _libraryPage.HideGame(gameName);
        }

        [Then(@"I wont see ""([^""]*)""")]
        public void ThenIWontSee(string gameName)
        {
            _libraryPage.Refresh();
            _libraryPage.ContainsGame(gameName).Should().BeFalse();
        }

        [Then(@"I click on the unhide button for ""([^""]*)""")]
        public void ThenIClickOnTheUnhideButtonFor(string gameName)
        {
            _libraryPage.Unhide(gameName);
        }

    }
}
