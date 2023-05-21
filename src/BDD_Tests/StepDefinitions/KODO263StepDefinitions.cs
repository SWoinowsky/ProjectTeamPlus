using BDD_Tests.PageObjects;
using Microsoft.Extensions.Configuration;
using System;
using BDD_Tests.Drivers;
using TechTalk.SpecFlow;
using NUnit.Framework;
using OpenQA.Selenium;
using BDD_Tests.Shared;
using SpecFlow.Actions.Selenium;
using BrowserDriver = BDD_Tests.Drivers.BrowserDriver;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public class KODO263StepDefinitions
    {


        private IConfigurationRoot Configuration { get; }

        private readonly ScenarioContext _scenarioContext;
        private readonly CompeteCreatePageObject _competeCreatePage;
        private readonly CompetitionDetailsPageObject _competitionDetailsPage;
        private readonly BrowserDriver _browserDriver; // Add this line

        private int _competitionId;

        public KODO263StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _competeCreatePage = new CompeteCreatePageObject(browserDriver.Current);
            _competitionDetailsPage = new CompetitionDetailsPageObject(browserDriver.Current);

            _scenarioContext = context;
            _browserDriver = browserDriver; // Assign the browserDriver parameter to the _browserDriver field

            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<KODO263StepDefinitions>();
            Configuration = builder.Build();
        }


        [Given(@"a competition I participated in has ended")]
        public void GivenACompetitionIParticipatedInHasEnded()
        {
            Thread.Sleep(500);

            _competeCreatePage.SelectCompetitionType("Duel"); 
            _competeCreatePage.SelectFriend("Hellfirecw");
            _competeCreatePage.SelectGame("Aim Lab"); 

            string startTime =
                DateTime.Today.AddDays(-30).ToString("yyyy-MM-ddTHH:mm"); // start date 30 days in the past
            string endTime = DateTime.Today.AddDays(-1).ToString("yyyy-MM-ddTHH:mm"); // end date 1 day in the past

            _competeCreatePage.SetCompetitionStartTime(startTime);
            _competeCreatePage.SetCompetitionEndTime(endTime);

            _competitionId = _competeCreatePage.SubmitCompetition(); 
        }


        [When(@"I visit the competition details page")]
        public void WhenIVisitTheCompetitionDetailsPage()
        {
            // This is where you get your competitionId value. It could be from anywhere as per your test design.
            int competitionId = _competitionId;

            // Use the UrlFor method with the competition ID.
            string competitionDetailsUrl = Common.UrlFor("CompetitionDetails", competitionId);

            _browserDriver.Current.Navigate().GoToUrl(competitionDetailsUrl);
        }



        [Then("I should see an option to vote on whether to participate in another competition")]
        public void ThenIShouldSeeAnOptionToVoteOnWhetherToParticipateInAnotherCompetition()
        {
            _competitionDetailsPage.VoteAgainButton.Should()
                .NotBeNull("because there should be an option to vote to participate in another competition");
        }

        [Given(@"half of the participants have voted to play again in the competition I participated in")]
        public void GivenHalfOfTheParticipantsHaveVotedToPlayAgainInTheCompetitionIParticipatedIn()
        {

            _competitionDetailsPage.ClickVoteAgainButton();
        }

        [Then(@"I should see an option to vote on a game for the next competition")]
        public void ThenIShouldSeeAnOptionToVoteOnAGameForTheNextCompetition()
        {
            var isModalPresent = _competitionDetailsPage.GameSelectModal != null;
            Assert.IsTrue(isModalPresent, "Game voting option is not visible.");
        }

        [Given(@"participants have voted on a game for the next competition")]
        public void GivenParticipantsHaveVotedOnAGameForTheNextCompetition()
        {
            _competitionDetailsPage.VoteForGame("242"); // 242 is the id of the game you want to vote for
            var gameCard = _competitionDetailsPage.GameCards.First(card => card.GetAttribute("id") == "242");
            var voteCount = int.Parse(gameCard.FindElement(By.CssSelector(".vote-count")).Text.Split(':').Last());
            Assert.IsTrue(voteCount > 0, $"No votes found for game with id 242.");
        }



        [Then(@"the game with the most votes should be selected as the focus of the next competition")]
        public void ThenTheGameWithTheMostVotesShouldBeSelectedAsTheFocusOfTheNextCompetition()
        {
            var gameWithMostVotes = _competitionDetailsPage.GameCards
                .OrderByDescending(card => int.Parse(card.FindElement(By.CssSelector(".vote-count")).Text.Split(':').Last()))
                .First();

            var selectedGame = _competitionDetailsPage.GameCards
                .FirstOrDefault(card => card.GetAttribute("data-vote-status") == "true");

            Assert.IsNotNull(selectedGame, "No game has been selected for the next competition.");
            Assert.AreEqual(gameWithMostVotes.GetAttribute("id"), selectedGame.GetAttribute("id"), "The game with most votes is not the selected game for the next competition.");
        }



    }
}
