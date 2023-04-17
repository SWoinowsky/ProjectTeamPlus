using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO12StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly HomePageObject _homePage;
        private readonly ScenarioContext _scenarioContext;
        private readonly LibraryPageObject _libraryPage;

        public KODO12StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _scenarioContext = context;
            _homePage = new HomePageObject(browserDriver.Current);
            _libraryPage = new LibraryPageObject(browserDriver.Current);

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO30StepDefinitions>();
            Configuration = builder.Build();
        }

        [Given(@"I am not signed in")]
        public void GivenIAmNotSignedIn()
        {
            // Do nothing!
        }

        [Then(@"The page title contains ""([^""]*)""")]
        public void ThenThePageTitleContains(string library)
        {
            _libraryPage.GetTitle().Should().ContainEquivalentOf(library, AtLeast.Once());
        }


        [Then(@"The page shows me a message")]
        public void ThenThePageShowsMeAMessage()
        {
            _libraryPage.GetLinkingMessage();
        }
    }
}
