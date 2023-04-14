using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BDD_Tests.Shared;
using System.Collections.ObjectModel;

namespace BDD_Tests.PageObjects
{
    public class SteamLoginPageObject : PageObject
    {
        public SteamLoginPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Steam Community";
        }

        public IWebElement RegisterButton => _webDriver.FindElement(By.Id("register-link"));
        public IWebElement NavBarHelloLink => _webDriver.FindElement(By.CssSelector("a[href=\"/Identity/Account/Manage\"]"));
        public IWebElement SteamLinkSignInButton => _webDriver.FindElement(By.Id("imageLogin"));

        public void SteamLinkButtonClick()
        {
            Thread.Sleep(5000);
            SteamLinkSignInButton.Click();
        }

    }
}
