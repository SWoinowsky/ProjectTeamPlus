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
    public class KODO259StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly HomePageObject _homePage;
        private readonly CompetePageObject _competePage;

        public KODO259StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _competePage = new CompetePageObject(browserDriver.Current);


            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO129StepDefinitions>();
            Configuration = builder.Build();
        }

        [When(@"I click on the compete create link")]
        public void WhenIClickOnTheCompeteCreateLink()
        {
            Thread.Sleep(1000);
            _competePage.ClickCompeteCreateLink();
            Thread.Sleep(1000);
        }


        [Then(@"I land on the compete create page")]
        public void ThenILandOnTheCompeteCreatePage()
        {
            var title = _competePage.GetTitle();
            title.Should().ContainEquivalentOf("Creation", AtLeast.Once());
        }

        //[Then(@"I can see Eithne's username")]
        //public void ThenICanSeeEithneUsername()
        //{
        //    _friendPage.GetFriendUsername().GetAttribute("innerHTML").Should().ContainEquivalentOf("Eithne", AtLeast.Once());
        //}

    }
}
