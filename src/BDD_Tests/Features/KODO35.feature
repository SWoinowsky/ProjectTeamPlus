@Justin
Feature: Ability to hide games from the library

A user needs to have an option to hide and unhide a game in the library so that it doesn't show up in the main list anymore.
Going to smash these two cases together in one go since it would break the code if unhide ran before hide.

@HideGames
Scenario: Library page contains button on game that allows you to hide them
	Given I am signed in
	When I click on the library link
	Then I should see a button to hide "Vampire Survivors"

@HideGames
Scenario: User can hide and unhide a game on the library page
	Given I am signed in
	When I click on the library link
	And I click on the hide button for "Vampire Survivors"
	Then I wont see "Vampire Survivors"
	And I click on the unhide button for "Vampire Survivors"
	And I should see my owned game "Vampire Survivors"
