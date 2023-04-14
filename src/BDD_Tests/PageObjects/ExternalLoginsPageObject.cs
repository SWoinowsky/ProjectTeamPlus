using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BDD_Tests.Shared;
using System.Collections.ObjectModel;

namespace BDD_Tests.PageObjects
{
    public class ExternalLoginsPageObject : PageObject
    {
        public ExternalLoginsPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "External Logins";
        }

        public IWebElement RegisterButton => _webDriver.FindElement(By.Id("register-link"));
        public IWebElement NavBarHelloLink => _webDriver.FindElement(By.CssSelector("a[href=\"/Identity/Account/Manage\"]"));
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

        public IWebElement SteamLinkButton => FindElementSafely(By.Id("link-login-button-Steam"));

        public IWebElement SteamUnLinkButton => FindElementSafely(By.Id("removeButton"));

        public IWebElement SteamIdTd => FindElementSafely(By.Id("login-provider-Steam"));


        public void SteamLinkButtonClick()
        {
            if (SteamUnLinkButton != null)
            {
                SteamUnLinkButtonClick();
            }
            SteamLinkButton.Click();
        }

        public void SteamUnLinkButtonClick()
        {
            SteamUnLinkButton.Click();
        }

        public bool SteamLinkButtonIsVisible()
        {
            return SteamLinkButton != null;
        }

        public bool SteamIdIsVisible()
        {
            return SteamIdTd != null;
        }

    }
}
