using System;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO138StepDefinitions
    {
        [When(@"I enter ""([^""]*)"" in the search box")]
        public void WhenIEnterInTheSearchBox(string name)
        {
            throw new PendingStepException();
        }

        [Then(@"I should see their friend card on the page")]
        public void ThenIShouldSeeTheirFriendCardOnThePage()
        {
            throw new PendingStepException();
        }
    }
}
