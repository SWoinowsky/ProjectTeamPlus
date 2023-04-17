using System;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO167_2StepDefinitions
    {
        [When(@"I enter a invalid email ""([^""]*)""")]
        public void WhenIEnterAInvalidEmail(string address)
        {
            throw new PendingStepException();
        }

        [Then(@"I should see a error message and the modal is still open")]
        public void ThenIShouldSeeAErrorMessageAndTheModalIsStillOpen()
        {
            throw new PendingStepException();
        }
    }
}
