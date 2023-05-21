using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecFlow.Actions.Selenium;
using System;

namespace BDD_Tests.PageObjects
{
    public class CompeteCreatePageObject : PageObject
    {
        private readonly IWebDriver _browser; // Add this line

        public CompeteCreatePageObject(IWebDriver webDriver) : base(webDriver)
        {
            _browser = webDriver; // Assign the webDriver to the _browser variable
            _pageName = "CompeteCreate";
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


        public void SelectCompetitionType(string competitionType)
        {
            var selectElement = new SelectElement(CategorySelect);
            selectElement.SelectByText(competitionType);
        }

        public void SetCompetitionStartTime(string startTime)
        {
            CompStartTime.Clear();
            CompStartTime.SendKeys(startTime);
        }

        public void SetCompetitionEndTime(string endTime)
        {
            CompEndTime.Clear();
            CompEndTime.SendKeys(endTime);
        }

        public void SelectFriend(string friendName)
        {
            var selectElement = new SelectElement(FriendSelector);
            selectElement.SelectByText(friendName);
        }

        public void SelectGame(string gameName)
        {
            var selectElement = new SelectElement(GameSelector);
            selectElement.SelectByText(gameName);
        }

        public int SubmitCompetition()
        {
            BeginCompetitionButton.Click();

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
