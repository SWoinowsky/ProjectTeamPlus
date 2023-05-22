using OpenQA.Selenium;

namespace BDD_Tests.PageObjects
{
    public class CompetitionDetailsPageObject : PageObject
    {
        public CompetitionDetailsPageObject(IWebDriver webDriver) : base(webDriver)
        {
            _pageName = "CompetitionDetails";
        }

        public IWebElement BackButton => FindElementSafely(By.CssSelector("a.btn.btn-info.back-btn"));
        public IWebElement GameImage => FindElementSafely(By.CssSelector("img.gameImage"));
        public IWebElement Achievements => FindElementSafely(By.CssSelector("div.achievementDiv"));
        public IWebElement DeleteCompetitionButton => FindElementSafely(By.Id("compDeleteBtn"));
        public IWebElement VoteAgainButton => FindElementSafely(By.Id("voteAgainBtn"));
        public IWebElement Competitors => FindElementSafely(By.CssSelector("div.card"));
        public IWebElement Leaderboard => FindElementSafely(By.Id("CompletionHistory"));

        public IWebElement GameSelectModal => FindElementSafely(By.Id("gameSelectModal"));
        public IWebElement GameSelectModalCloseButton => FindElementSafely(By.CssSelector("button[data-dismiss='gameSelectModal']"));

        public IEnumerable<IWebElement> GameCards => FindElementsSafely(By.CssSelector("div.card"));

        public IWebElement VoteCount => FindElementSafely(By.Id("voteCount"));


        public void ClickBackButton()
        {
            BackButton?.Click();
        }

        public void ClickDeleteCompetitionButton()
        {
            DeleteCompetitionButton?.Click();
        }

        public void ClickVoteAgainButton()
        {
            VoteAgainButton?.Click();
        }

        public string GetGameImageSrc()
        {
            return GameImage?.GetAttribute("src");
        }

        public string GetFirstAchievementText()
        {
            return Achievements?.FindElement(By.CssSelector("p.card-title")).Text;
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

        public void VoteForGame(string gameId)
        {
            var gameCard = FindElementSafely(By.Id(gameId));
            gameCard?.Click();
        }


        private IEnumerable<IWebElement> FindElementsSafely(By by)
        {
            try
            {
                return _webDriver.FindElements(by);
            }
            catch (NoSuchElementException)
            {
                return Enumerable.Empty<IWebElement>();
            }
        }



    }
}