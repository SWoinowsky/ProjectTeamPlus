using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BDD_Tests.Shared;
using System.Collections.ObjectModel;

namespace BDD_Tests.PageObjects
{
    public class LibraryPageObject : PageObject
    {
        public LibraryPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Library";
        }

        public IWebElement RegisterButton => _webDriver.FindElement(By.Id("register-link"));
        public IWebElement NavBarHelloLink => _webDriver.FindElement(By.CssSelector("a[href=\"/Identity/Account/Manage\"]"));
        public IWebElement SteamLinkButton => _webDriver.FindElement(By.Id("link-login-button-Steam"));

        public IWebElement SteamAvatarImg => _webDriver.FindElement(By.ClassName("user-avatar"));

        public void SteamLinkButtonClick()
        {
            SteamLinkButton.Click();
        }

        public bool SteamAvatarImgVisible()
        {
            return SteamAvatarImg != null;
        }
        public void Logout()
        {
            IWebElement navbarLogoutButton = _webDriver.FindElement(By.Id("logout-button"));
            navbarLogoutButton.Click();
        }
    }
}
