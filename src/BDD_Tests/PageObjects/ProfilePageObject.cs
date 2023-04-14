using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BDD_Tests.Shared;
using System.Collections.ObjectModel;

namespace BDD_Tests.PageObjects
{
    public class ProfilePageObject : PageObject
    {
        public ProfilePageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Profile";
        }

        public IWebElement RegisterButton => _webDriver.FindElement(By.Id("register-link"));
        public IWebElement NavBarHelloLink => _webDriver.FindElement(By.CssSelector("a[href=\"/Identity/Account/Manage\"]"));
        public IWebElement FriendsList => _webDriver.FindElement(By.Id("friendsListDiv"));
        public IWebElement ProfileImage => _webDriver.FindElement(By.Id("divSteamAvatar"));
        public IWebElement UsernameAndLevel => _webDriver.FindElement(By.Id("divSteamNameLevel"));
        public IWebElement EithnéPageLink => _webDriver.FindElement(By.Id("$Eithné of Brokiloén"));

        public IWebElement SteamLinkPage => _webDriver.FindElement(By.Id("external-login"));


        public bool FriendsListVisible()
        {
            return (FriendsList != null);
        }

        public bool ProfileImageVisible()
        {
            return (ProfileImage != null);
        }

        public bool UsernameAndLevelVisible()
        {
            return (UsernameAndLevel != null);
        }

        public void EithnéPageClick()
        {
            EithnéPageLink.Click();
        }

        public void SteamLinkPageClick()
        {
            SteamLinkPage.Click();
        }

        public void Logout()
        {
            IWebElement navbarLogoutButton = _webDriver.FindElement(By.Id("logout-button"));
            navbarLogoutButton.Click();
        }
    }
}
