using BDD_Tests.Drivers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDD_Tests.PageObjects;
using OpenQA.Selenium;
using BoDi;
using OpenQA.Selenium.Support.UI;
using BDD_Tests.StepDefinitions;
using Microsoft.Extensions.Configuration;

namespace BDD_Tests.Hooks
{
    /// <summary>
    /// Share the same browser window for all scenarios
    /// </summary>
    /// <remarks>
    /// This makes the sequential execution of scenarios faster (opening a new browser window each time would take more time)
    /// As a tradeoff:
    ///  - we cannot run the tests in parallel
    ///  - we have to "reset" the state of the browser before each scenario
    /// </remarks>
    [Binding]
    public class SharedBrowserHooks
    {
        private static IObjectContainer _objectContainer;
        private static BrowserDriver _browserDriver;

        private IConfigurationRoot Configuration { get; }
        private readonly ScenarioContext _scenarioContext;

        public SharedBrowserHooks(ScenarioContext scenarioContext, IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<SharedBrowserHooks>();
            Configuration = builder.Build();
        }

        [BeforeTestRun]
        public static void BeforeTestRun(IObjectContainer container)
        {

            _browserDriver = new BrowserDriver();
            container.RegisterInstanceAs<BrowserDriver>(_browserDriver);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            if (_browserDriver != null)
            {
                _browserDriver.Dispose();
            }
        }


        [AfterScenario("@RevertNickname")]
        public void TearDown()
        {
            var browserDriver = _objectContainer.Resolve<BrowserDriver>();
            var webDriver = browserDriver.WebDriver;
            var friendsPage = new FriendsPageObject(webDriver);

            var allFriendIds = friendsPage.GetAllFriendSteamIds();

            foreach (var friendId in allFriendIds)
            {
                try
                {
                    friendsPage.Revert(friendId);
                }
                catch (NoSuchElementException)
                {
                    // Do nothing if the revert icon isn't found for a friend
                }
            }
        }

        [BeforeScenario("@SetupAlias")]
        public void SetupAlias()
        {
            var browserDriver = _objectContainer.Resolve<BrowserDriver>();
            var webDriver = browserDriver.WebDriver;

            // Define the base URL of your application
            var baseUrl = "https://localhost:7123";

            // Navigate to the Login page
            webDriver.Navigate().GoToUrl($"{baseUrl}/Identity/Account/Login");

            var loginPage = new LoginPageObject(webDriver);

            var user = new TestUser
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

            loginPage.EnterEmail(user.Email);
            loginPage.EnterPassword(user.Password);
            loginPage.Login();

            // Navigate to the Friends page
            webDriver.Navigate().GoToUrl($"{baseUrl}/Home/Friends");

            var friendsPage = new FriendsPageObject(webDriver);
            friendsPage.GiveNewName("Steve", "Minecraft");

            // Log out
            friendsPage.Logout();  // Assuming the Logout method is in your FriendsPageObject

            // Navigate back to the Login page
            webDriver.Navigate().GoToUrl($"{baseUrl}/Identity/Account/Login");
        }



    }
}
