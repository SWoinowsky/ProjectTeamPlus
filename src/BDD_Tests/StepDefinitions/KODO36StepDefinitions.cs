using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO36StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly HomePageObject _homePage;
        private readonly ScenarioContext _scenarioContext;
        private readonly LibraryPageObject _libraryPage;
        private readonly LoginPageObject _loginPage;

        public KODO36StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _scenarioContext = context;
            _homePage = new HomePageObject(browserDriver.Current);
            _libraryPage = new LibraryPageObject(browserDriver.Current);
            _loginPage = new LoginPageObject(browserDriver.Current);

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO30StepDefinitions>();
            Configuration = builder.Build();
        }

        [When(@"I click on the hidden modal button")]
        public void WhenIClickOnTheHiddenModalButton()
        {
            
        }

        [Then(@"I should see the empty hidden modal")]
        public void ThenIShouldSeeTheEmptyHiddenModal()
        {
            throw new PendingStepException();
        }
    }
}
