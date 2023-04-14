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
        public IWebElement SteamLinkButton => _webDriver.FindElement(By.Id("link-login-button-Steam"));

        public IWebElement SteamUnLinkButton => _webDriver.FindElement(By.Id("removeButton"));

        public IWebElement SteamIdTd => _webDriver.FindElement(By.Id("login-provider-Steam"));

        public void SteamLinkButtonClick()
        {
            if (SteamUnLinkButton != null)
            {
                //Unlink old account if it already is linked
                SteamUnLinkButton.Click();
            }
            SteamLinkButton.Click();
        }
        public bool SteamIdIsVisible()
        {
            return SteamIdTd != null;
        }

    }
}
