@Justin
Feature: As a user, I want to be able to start a time trial race for a specific game

Users need to have the ability to create a speed run challenge from the compete page just like the achievement battles.

@LoggedIn
Scenario: Users will see a button to create the speed run challenge
	Given I am signed in
	When I click on the "Compete" link
	And I click on the create speed run link
	Then I land on the speed run create page

@LoggedIn
Scenario: Users are able to click the create speed run button and a modal pops up
	Given I am signed in
	When I click on the "Compete" link
	And I click on the create speed run link
	Then I will see the create speed run modal

@LoggedIn
Scenario: Users are able to create a speed run, and delete it
	Given I am signed in
	When I click on the "Compete" link
	And I click on the create speed run link
	And I input valid info for the game "Vampire Survivors"
	And A competition for "Vampire Survivors" is made
	Then I can click on the competition made
	And I can delte it