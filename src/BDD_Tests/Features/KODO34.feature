@Cole
Feature: AbilityForAUserToSeeDashboard

As a user, I want to see a nice dashboard that will operate as the news hub, so that I can manage what games are being tracked and see new's updates for games that I follow.

This one assumes a single user has already created an account and linked it with steam which should be set up in the background

@LoggedIn
Scenario: I am a test user and I want to make sure my Dashboard page contains Steam name, SteamImg, and Level for my linked account
	Given I am signed in
	When I click on the dashboard link
	Then I end up on the dashboard page
	And I can see my Steam information displayed on the dashboard

@LoggedIn
Scenario: I am a test user and I want to make sure dashboard page displays the recent games carousel
	Given I am signed in
	When I click on the dashboard link
	Then I should see my recent games carousel

@LoggedIn
Scenario: I am a test user and I want to make sure dashboard page shows the followed games carousel after I follow a game
	Given I am signed in
	When I click on the library link
	And I should see and be able to follow a game
	And I click on the dashboard link
	Then I should see my followed games carousel
	And I click on the library link
	And I should see and be able to unFollow that same game




