using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BDD_Tests.Shared;
using System.Collections.ObjectModel;
using BDD_Tests.Drivers;
using SpecFlow.Actions.Selenium;

namespace BDD_Tests.PageObjects
{
    public class FriendsPageObject : PageObject
    {
        public FriendsPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Friends";
        }

        public IWebElement Invite => _webDriver.FindElement(By.Id("inv-friend-i"));
        public IWebElement InviteModal => _webDriver.FindElement(By.Id("sendInviteModal"));
        public IWebElement EmailBox => _webDriver.FindElement(By.Id("email-input"));
        public IWebElement SendBtn => _webDriver.FindElement(By.Id("send-inv"));
        public IWebElement InviteResult => _webDriver.FindElement(By.Id("email-error"));
        public IWebElement FriendSearch => _webDriver.FindElement(By.Id("search-input"));

        public IWebElement FriendNamedSteve => _webDriver.FindElement(By.ClassName("friend-card"));

        public List<string> GetAllFriendSteamIds()
        {
            Thread.Sleep(500);
            var allFriendElements = _webDriver.FindElements(By.ClassName("friend-persona"));

            var allFriendIds = new List<string>();

            foreach (var friendElement in allFriendElements)
            {
                var friendId = friendElement.GetAttribute("id");
                if (!string.IsNullOrEmpty(friendId))
                {
                    friendId = friendId.Replace("persona-", "");
                    allFriendIds.Add(friendId);
                }
            }

            return allFriendIds;
        }




        public IWebElement GetFriendCardByName(string friendName)
        {
            return _webDriver.FindElement(By.Id(friendName));
        }

        public IWebElement NicknameTextBox => _webDriver.FindElement(By.ClassName("friend-name"));

        public IWebElement GetRevertButtonBySteamId(string friendId)
        {
            Thread.Sleep(500);
            return _webDriver.FindElement(By.CssSelector($"i[data-revert='{friendId}']"));

        }


        public void ClickName(string friendName)
        {
            GetFriendCardByName(friendName).Click();
        }

        public void GiveNewName(string friendName, string alias)
        {

            IWebElement friendCard = GetFriendCardByName(friendName);
            IWebElement friendNameElement = friendCard.FindElement(By.ClassName("friend-name"));
            friendNameElement.Click();

            Thread.Sleep(500);
            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            IWebElement NicknameTextBox = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("name-box")));
            
            Thread.Sleep(500);
            NicknameTextBox.SendKeys(alias);
            Thread.Sleep(500);
            NicknameTextBox.SendKeys(Keys.Enter);
        }



        public string NewName(string friendName)
        {
            return _webDriver.FindElement(By.Id($"name-{friendName}")).Text;
        }




        public void Revert(string friendId)
        {
            GetRevertButtonBySteamId(friendId).Click();
        }

    


        public void Logout()
        {
            IWebElement navbarLogoutButton = _webDriver.FindElement(By.Id("logout-button"));
            navbarLogoutButton.Click();
        }
    }
}
