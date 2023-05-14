@Cole
Feature: AbilityForAUserToLinkSteamAccount
This Bdd Test use to be for this user story:
As a user, I want to be able to log-in with my Steam identity, so that I can securely access my user information.


But now simply verifies that the steamId has been seeded to the Test User that is used for all of these tests
Mainly as a sanity check due to steam only having 24 hour cookies for logging in

@LoggedIn
Scenario: I am a test user and I want to link my steam account to my user account
	Given I am signed in
	And I click on the "Profile" link
	And I click on the Steam Account link
	Then I should see my SteamId displayed
	
Scenario: I am a test user and I want to ensure my steam information has been pulled in correctly
	Given I am signed in
	When I click on the "Library" link
	Then I should see my owned game "Aim Lab"
	

	
