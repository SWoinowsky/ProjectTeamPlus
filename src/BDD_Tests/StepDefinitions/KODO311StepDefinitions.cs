using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO311StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly CompetePageObject _competePage;

        public KODO311StepDefinitions(BrowserDriver browserDriver)
        {
            _competePage = new CompetePageObject(browserDriver.Current);

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO129StepDefinitions>();
            Configuration = builder.Build();
        }

        [When(@"I click on the create speed run link")]
        public void WhenIClickOnTheCreateSpeedRunLink()
        {
            _competePage.ClickCreateSpeedRunLink();
        }

        [Then(@"I land on the speed run create page")]
        public void ThenILandOnTheSpeedRunCreatePage()
        {
            var title = _competePage.GetTitle();
            title.Should().ContainEquivalentOf("Creation", AtLeast.Once());
        }
    }
}
