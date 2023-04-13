using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;

namespace BDD_Tests.StepDefinitions
{
    public class TestUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }

    [Binding]
    public class KODO129StepDefinitions
    {
        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPageObject _loginPage;
        private readonly HomePageObject _homePage;
        private readonly CompetePageObject _competePage;

        public KODO129StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _homePage = new HomePageObject(browserDriver.Current);
            _competePage = new CompetePageObject(browserDriver.Current);
            _scenarioContext = context;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO129StepDefinitions>();
            Configuration = builder.Build();
        }


        [Given(@"I am signed in")]
        public void GivenIAmSignedIn()
        {
            TestUser user = new TestUser
            {
                UserName = "TestUser",
                FirstName = "The",
                LastName = "Admin",
                Email = "TestUser@mail.com",
                Password = Configuration["SeedUserPW"]
            };
            if (user.Password == null)
            {
                throw new Exception("Did you forget to set the admin password in user-secrets?");
            }
            Debug.WriteLine("Password = " + user.Password);

            // Go to the login page
            _loginPage.GoTo();
            //Thread.Sleep(3000);
            // Now (attempt to) log them in.  Assumes previous steps defined the user
            
            _loginPage.EnterEmail(user.Email);
            _loginPage.EnterPassword(user.Password);
            _loginPage.Login();
        }

        [When(@"I click on the compete link")]
        public void WhenIClickOnTheCompeteLink()
        {
            _homePage.ClickNavBarCompeteLink(); // it only works if you tell it twice, code moment
            Thread.Sleep(1000);
            _homePage.ClickNavBarCompeteLink();
            Thread.Sleep(1000);
        }

        [Then(@"Then I end up on the competitions list page")]
        public void ThenIEndUpOnCompetitionsListPage()
        {
            var title = _competePage.GetTitle();
            _competePage.GetTitle().Should().ContainEquivalentOf("Competitions", AtLeast.Once());
        }
    }
}
