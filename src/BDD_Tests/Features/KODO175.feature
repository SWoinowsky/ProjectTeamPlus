@Carlos

Feature: Ability to assign a nickname to a friend on the Friends page

A user may want to be able to assign nicknames to their friends on the platform for whatever reason

Scenario: I am a user with a friend on the Friends page wanting to give them a nickname
	Given I have a friend on the friends page
	And I click on the name on the friend card
	When I enter a nickname "Alias"
	Then I should see them references as "Alias"