using OpenQA.Selenium;
using BDD_Tests.PageObjects;

namespace BDD_Tests.PageObjects
{
    public class DashboardPageObject : PageObject
    {
        public DashboardPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Dashboard";
        }

        private IWebElement FindElementSafely(By by)
        {
            try
            {
                return _webDriver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }


        public IWebElement ToDoButton => _webDriver.FindElement(By.Id("toDoButton"));

        public IWebElement ToDoListDiv => _webDriver.FindElement(By.Id("toDoCollapse"));

        public IWebElement ProfileImage => _webDriver.FindElement(By.ClassName("user-avatar"));

        public IWebElement SteamLevel => _webDriver.FindElement(By.Id("divSteamNameLevel"));

        public IWebElement SteamUserName => _webDriver.FindElement(By.Id("userName"));

        public IWebElement RecentGamesCarousel => FindElementSafely(By.Id("recentGamesCarousel"));

        public IWebElement FollowedGamesCarousel => FindElementSafely(By.Id("followedGamesCarousel"));


        public IWebElement NavBarHelloLink => _webDriver.FindElement(By.CssSelector("a[href=\"/Identity/Account/Manage\"]"));

        public string NavbarWelcomeText()
        {
            return NavBarHelloLink.Text;
        }

        public bool ProfileImageVisible()
        {
            return (ProfileImage != null);
        }
        public bool UsernameAndLevelVisible()
        {
            return (SteamUserName != null && SteamLevel != null);
        }

        public bool RecentGamesIsVisible()
        {
            return RecentGamesCarousel != null;
        }
        public bool FollowedGamesIsVisible()
        {
            return FollowedGamesCarousel != null;
        }


        public void Logout()
        {
            IWebElement navbarLogoutButton = _webDriver.FindElement(By.Id("logout-button"));
            navbarLogoutButton.Click();
        }
    }
}
