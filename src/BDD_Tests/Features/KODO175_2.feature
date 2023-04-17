@Carlos

Feature: Ability to revert a nickname given to a friend on the Friends page

A user may want to be able to change their friends nickname back to the original on the platform for whatever reason

@LoggedIn
Scenario: I am a user with a friend on the Friends page wanting to remove the nickname I gave them
	Given I have a friend on the friends page
	And That friend has a set nickname
	When I click on the revert button
	Then I should see their name return to original