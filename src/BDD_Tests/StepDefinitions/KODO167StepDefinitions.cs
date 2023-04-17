using System;
using TechTalk.SpecFlow;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO167StepDefinitions
    {
        [Given(@"I click on the envelope icon")]
        public void GivenIClickOnTheEnvelopeIcon()
        {
            throw new PendingStepException();
        }

        [When(@"I enter a valid email ""([^""]*)""")]
        public void WhenIEnterAValidEmail(string address)
        {
            throw new PendingStepException();
        }

        [Then(@"I should see a success message and the modal has closed")]
        public void ThenIShouldSeeASuccessMessageAndTheModalHasClosed()
        {
            throw new PendingStepException();
        }
    }
}
