using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BDD_Tests.Shared;
using System.Collections.ObjectModel;

namespace BDD_Tests.PageObjects
{
    public class FriendsPageObject : PageObject
    {
        public FriendsPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Friends";
        }

        public IWebElement CardName => _webDriver.FindElement(By.ClassName("friend-name"));
        public IWebElement FriendNamedSteve => _webDriver.FindElement(By.Id("76561199093267477"));
        public IWebElement NicknameTextBox => _webDriver.FindElement(By.ClassName("name-box"));
        public IWebElement Revert => _webDriver.FindElement(By.ClassName("revert"));
        public IWebElement Invite => _webDriver.FindElement(By.Id("inv-friend-i"));
        public IWebElement InviteModal => _webDriver.FindElement(By.Id("sendInviteModal"));
        public IWebElement EmailBox => _webDriver.FindElement(By.Id("email-input"));
        public IWebElement SendBtn => _webDriver.FindElement(By.Id("send-inv"));
        public IWebElement InviteResult => _webDriver.FindElement(By.Id("email-error"));
        public IWebElement FriendSearch => _webDriver.FindElement(By.Id("search-input"));


        public void ClickName()
        {
            FriendNamedSteve.Click();
        }

        public void GiveNewName(string alias)
        {
            NicknameTextBox.Click();
            NicknameTextBox.SendKeys(alias);
            NicknameTextBox.SendKeys(Keys.Enter);
        }

        public string NewName()
        {
            return FriendNamedSteve.Text;
        }


        public void Logout()
        {
            IWebElement navbarLogoutButton = _webDriver.FindElement(By.Id("logout-button"));
            navbarLogoutButton.Click();
        }
    }
}
