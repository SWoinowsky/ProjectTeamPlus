using System;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class HelloWorldStepDefinitions
    {
        [Given(@"I am a visitor")]
        public void GivenIAmAVisitor()
        {
            // Nothing to do!
        }

        [When(@"I am on the ""([^""]*)"" page")]
        public void WhenIAmOnThePage(string home)
        {
            throw new PendingStepException();
        }

        [Then(@"The page title contains the word ""([^""]*)""")]
        public void ThenThePageTitleContainsTheWord(string home)
        {
            throw new PendingStepException();
        }
    }
}
