using BDD_Tests.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecFlow.Actions.Selenium;
using System;

namespace BDD_Tests.PageObjects
{
    public class CompeteCreatePageObject : PageObject
    {
        private readonly IWebDriver _browser; 

        public CompeteCreatePageObject(IWebDriver webDriver) : base(webDriver)
        {
            _browser = webDriver;
            _pageName = "CompeteCreate";
        }

        public void GoTo()
        {
            string url = Common.UrlFor("Create Competition");
            _webDriver.Navigate().GoToUrl(url);
        }


        public IWebElement CategorySelect => FindElementSafely(By.Id("categorySelect"));
        public IWebElement CompStartTime => FindElementSafely(By.Name("CompStartTime"));
        public IWebElement CompEndTime => FindElementSafely(By.Name("CompEndTime"));
        public IWebElement CreateForm => FindElementSafely(By.Id("createForm"));

        // Additional elements
        public IWebElement DuelDiv => FindElementSafely(By.Id("DuelDiv"));
        public IWebElement FfaDiv => FindElementSafely(By.Id("ffaDiv"));
        public IWebElement FriendSelector => FindElementSafely(By.Id("friendSelector"));
        public IWebElement GroupSelector => FindElementSafely(By.Id("groupSelector"));
        public IWebElement GameSelector => FindElementSafely(By.Id("gameSelector"));
        public IWebElement AchievementDiv => FindElementSafely(By.Id("AchievementDiv"));

     
        public IWebElement BeginCompetitionButton => FindElementSafely(By.Id("compCreateSubmit"));



        public static Func<IWebDriver, bool> ElementIsVisible(By locator)
        {
            return (driver) =>
            {
                try
                {
                    return driver.FindElement(locator).Displayed;
                }
                catch (Exception)
                {
                    // Returns false because the element is not present or not visible.
                    return false;
                }
            };
        }

        public IWebElement WaitForElementToBeVisible(By locator, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(ElementIsVisible(locator)) ? _webDriver.FindElement(locator) : null;
            }
            return _webDriver.FindElement(locator);
        }

        public void SelectCompetitionType(string competitionType)
        {
            var selectElement = new SelectElement(WaitForElementToBeVisible(By.Id("categorySelect"), 10));
            selectElement.SelectByText(competitionType);
        }

        public void SetCompetitionStartTime(string startTime)
        {
            var startTimeElement = WaitForElementToBeVisible(By.Name("CompStartTime"), 10);
            startTimeElement.Clear();
            startTimeElement.SendKeys(startTime);
        }

        public void SetCompetitionEndTime(string endTime)
        {
            var endTimeElement = WaitForElementToBeVisible(By.Name("CompEndTime"), 10);
            endTimeElement.Clear();
            endTimeElement.SendKeys(endTime);
        }

        public void SelectFriend(string friendName)
        {
            var selectElement = new SelectElement(WaitForElementToBeVisible(By.Id("friendSelector"), 10));
            selectElement.SelectByText(friendName);
        }

        public void SelectGame(string gameName)
        {
            var selectElement = new SelectElement(WaitForElementToBeVisible(By.Id("gameSelector"), 10));
            selectElement.SelectByText(gameName);
        }

        public int SubmitCompetition()
        {
            var beginCompetitionButton = WaitForElementToBeVisible(By.Id("compCreateSubmit"), 10);
            beginCompetitionButton.Click();

            // Assuming that the browser navigates to the details page of the competition just created.
            string currentUrl = _browser.Url;
            Uri uri = new Uri(currentUrl);

            // Assuming the ID is the last part of the URL
            string idString = uri.Segments.Last();

            if (int.TryParse(idString, out int competitionId))
            {
                return competitionId;
            }

            throw new Exception("Unable to parse competition ID from URL.");
        }

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

  
    }
}
