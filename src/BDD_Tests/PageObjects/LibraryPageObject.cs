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
        public ReadOnlyCollection<IWebElement> FollowGamesButtons => _webDriver.FindElements(By.ClassName("follow-btn"));

        public IWebElement LinkingMessage => _webDriver.FindElement(By.Id("link-message"));
        public IWebElement GameLibrary => _webDriver.FindElement(By.Id("game-Library"));
        public IWebElement HiddenModal => _webDriver.FindElement(By.Id("hidden-game-modal"));
        public IWebElement RefreshButton => _webDriver.FindElement(By.ClassName("refresh-btn"));
        public IWebElement ShowHiddenModalButton => _webDriver.FindElement(By.ClassName("show-hidden-btn"));
        public IWebElement EmptyModalMessage => HiddenModal.FindElement(By.Id("hidden-modal-no-game-message"));
        public IWebElement HiddenModalCloseButton => _webDriver.FindElement(By.ClassName("btn-close-white"));

        public void SteamLinkButtonClick()
        {
            SteamLinkButton.Click();
        }

        public void FollowFirstGame()
        {
            FollowGamesButtons.FirstOrDefault();

            if (FollowGamesButtons.Count > 0)
            {
                FollowGamesButtons.FirstOrDefault().Click();
            }
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

        public bool GetLinkingMessage()
        {
            return LinkingMessage != null;
        }

        public void Refresh()
        {
            RefreshButton.Click();
        }

        public bool ContainsGame(string gameName)
        {
            return GameLibrary.FindElement(By.Id(gameName)) != null;
        }

        public bool DoesNotContainGame(string gameName)
        {
            try
            {
                GameLibrary.FindElement(By.Id(gameName));
                return false;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        public void ShowHiddenModal()
        {
            ShowHiddenModalButton.Click();
        }

        public bool GetEmptyModalMessage()
        {
            return EmptyModalMessage != null;
        }

        public bool FindHideButtonForGame(string gameName)
        {
            return _webDriver.FindElement(By.Id(gameName)).FindElement(By.ClassName("hide-btn")) != null;
        }

        public void HideGame(string gameName)
        {
            _webDriver.FindElement(By.Id(gameName)).FindElement(By.ClassName("hide-btn")).Click();
        }

        public void Unhide(string gameName)
        {
            ShowHiddenModal();
            HiddenModal.FindElement(By.Id(gameName)).FindElement(By.ClassName("unhide-btn")).Click();
            HiddenModalCloseButton.Click();
        }
    }
}
