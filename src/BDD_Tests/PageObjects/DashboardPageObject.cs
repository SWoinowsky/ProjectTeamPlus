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

        public IWebElement FindElementSafely(By by)
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

        public IReadOnlyCollection<IWebElement> Badges =>
            _webDriver.FindElements(By.ClassName("badge-button-img"));

        public IReadOnlyCollection<IWebElement> BadgeDescriptions =>
            _webDriver.FindElements(By.ClassName("badge-description"));

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


 
        public bool IsBadgePresent()
        {
            return Badges.Count > 0;
        }

        public IWebElement GetBadgeByName(string badgeName)
        {
            return _webDriver.FindElement(By.XPath($"//img[@alt='{badgeName}']"));
        }

        public void ClickBadge(IWebElement badge)
        {
            badge.Click();
        }

        public string GetBadgeDescription(IWebElement badgeDescription)
        {
            return badgeDescription.Text;
        }

        public void GoToBadgeDetails(string badgeName)
        {
            var badge = GetBadgeByName(badgeName);
            if (badge != null)
            {
                ClickBadge(badge);
            }
        }
    }
}


