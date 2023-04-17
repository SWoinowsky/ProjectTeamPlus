@Justin
Feature: Ability to hide games from the library

A user needs to have an option to hide a game in the library so that it doesn't show up in the main list anymore.
Going to be making use of the unhide to get this to behave correctly, but that's not what's being tested.
It's necessary since it updates our DB with a hidden status.

@HideGames
Scenario: Library page contains button on game that allows you to hide them
	Given I am signed in
	When I click on the library link
	Then I should see a button to hide "Vampire Survivors"

@HideGames
Scenario: User can hide a game on the library page
	Given I am signed in
	When I click on the library link
	And I click on the hide button for "Vampire Survivors"
	Then I wont see "Vampire Survivors"
