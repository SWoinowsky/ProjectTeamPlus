@Cole
Feature: AbilityForAUserToRemoveSteamAccount

As a user, I want to be able to remove my Steam id from my SIN account, so that I can attach my account to a 
different Steam id and see my updated account information on the website. 

This BDD Test assumes the user has already linked their steam account their steam account 
It also assumes there is a seeded test user to link the steam account to

Once the steamId is removed, it is readded through this test to ensure everything is working for later tests since
our enviroment needs to have a steamID for a test user to actually do anything with our website.

@LoggedIn
Scenario: I am a test user and I want to unlink my steam account from my user account
	Given I am signed in
	When I click on the profile link
	And I click on the Steam Account link
	And I click on the Remove Steam link button
	Then I should see the button to link a steam account
	And I click on the Steam link button
	And I should be redirected to steams login page
	And I should be able to click sign in
	And I should be redirected back and see my library

