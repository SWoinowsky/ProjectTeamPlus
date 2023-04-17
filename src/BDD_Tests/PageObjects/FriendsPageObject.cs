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

        public IWebElement FriendNamedSteve => _webDriver.FindElement(By.Id("76561199093267477"));
        public IWebElement NicknameTextBox => _webDriver.FindElement(By.ClassName("name-box"));


        public void ClickName()
        {
            FriendNamedSteve.Click();
        }

        public void GiveNewName()
        {
            NicknameTextBox.Click();
            NicknameTextBox.SendKeys("Minecraft");
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
