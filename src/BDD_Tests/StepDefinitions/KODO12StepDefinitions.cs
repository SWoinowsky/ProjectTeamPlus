using BDD_Tests.Drivers;
using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
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
        private readonly LoginPageObject _loginPage;

        public KODO12StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _scenarioContext = context;
            _homePage = new HomePageObject(browserDriver.Current);
            _libraryPage = new LibraryPageObject(browserDriver.Current);
            _loginPage = new LoginPageObject(browserDriver.Current);

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO30StepDefinitions>();
            Configuration = builder.Build();
        }
        [Given(@"I am a user with name ""([^""]*)""")]

        public void GivenIAmAUserWithName(string p0)
        {
            TestUser user = new TestUser
            {
                UserName = p0,
                FirstName = "The",
                LastName = "User",
                Email = "TestUser2@mail.com",
                Password = Configuration["SeedUserPW"]
            };
            if (user.Password == null)
            {
                throw new Exception("Did you forget to set the user password in user-secrets?");
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

        [Then(@"I should see my owned games")]
        public void ThenIShouldSeeMyOwnedGames()
        {
            _libraryPage.ContainsAimLabGame();
        }

    }
}
