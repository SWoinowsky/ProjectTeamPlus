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

