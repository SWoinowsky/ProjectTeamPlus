using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO129StepDefinitions
    {

        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPageObject _loginPage;
        private readonly HomePageObject _homePage;

        public KODO129StepDefinitions(BrowserDriver browserDriver) 
        {
            _homePage = new HomePageObject(browserDriver.Current);
        }


        [Given(@"I am signed in")]
        public void GivenIAmAVisitor()
        {
            // Nothing to do!
        }

        [When(@"I am on the ""([^""]*)"" page")]
        public void WhenIAmOnThePage(string pageName)
        {
            _homePage.GoTo( pageName );
        }

        [Then(@"The page title contains the word ""([^""]*)""")]
        public void ThenThePageTitleContainsTheWord(string p0)
        {
            _homePage.GetTitle().Should().ContainEquivalentOf( p0, AtLeast.Once() );
        }
    }
}
