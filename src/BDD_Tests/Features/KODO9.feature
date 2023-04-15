@Cole
Feature: AbilityForAUserToLinkSteamAccount

As a user, I want to be able to log-in with my Steam identity, so that I can securely access my user information.

This BDD Test assumes the user has not linked their steam account yet and should handle loading the cookie from steam to link accounts.
It also assumes there is a seed test user to link the steam account to

@LoggedIn
Scenario: I am a test user and I want to link my steam account to my user account
	Given I am signed in
	And I click on the profile link
	And I click on the Steam Account link
	When I click on the Steam link button
	And I am redirected to steams login page
	And I am able to click sign in 
	And I am redirected back and see my library
	And I click on the profile link
	And I click on the Steam Account link
	Then I should see my SteamId displayed
	

	
