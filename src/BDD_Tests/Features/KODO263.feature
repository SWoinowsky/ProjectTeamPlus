@Cole
Feature: AbilityToVoteForNextCompetition

As a user, I want to vote for the next competition's game after a competition ends, so that I can continuously engage in challenges with my friends.

@LoggedIn
Scenario: I am a user and I want to vote on whether to participate in another competition after the current one ends
    Given I am signed in
    And a competition I participated in has ended
    When I visit the competition details page
    Then I should see an option to vote on whether to participate in another competition

@LoggedIn
Scenario: I am a user and I want to nominate a game for the next competition after a majority of participants vote to play again
    Given I am signed in
    And a majority of participants have voted to play again in the competition I participated in
    When I visit the competition details page
    Then I should see an option to nominate a game for the next competition

@LoggedIn
Scenario: I am a user and I want to vote on the nominated games for the next competition after a majority of participants vote to play again
    Given I am signed in
    And a majority of participants have voted to play again in the competition I participated in
    And a game has been nominated for the next competition
    When I visit the competition details page
    Then I should see an option to vote on the nominated games for the next competition

@LoggedIn
Scenario: I am a user and I want to see the game with the most votes as the focus of the next competition
    Given I am signed in
    And participants have nominated and voted on games for the next competition I participated in
    When I visit the competition details page
    Then the game with the most votes should be selected as the focus of the next competition
