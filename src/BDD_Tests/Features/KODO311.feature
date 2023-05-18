@Justin
Feature: As a user, I want to be able to start a time trial race for a specific game

Users need to have the ability to create a speed run challenge from the compete page just like the achievement battles.

@LoggedIn
Scenario: Users will see a button to create the speed run challenge
	Given I am signed in
	When I click on the "Compete" link
	And I click on the create speed run link
	Then I land on the speed run create page
