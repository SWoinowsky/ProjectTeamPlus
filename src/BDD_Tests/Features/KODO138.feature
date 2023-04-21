@Carlos

Feature: Ability to for a user on the friends page either by original or nickname

A user may want to be able to search for a friend on the friends page if they have a lot

@LoggedIn
Scenario: I am a user with a friend on the Friends page and I want to find them easily
	Given I have a friend on the friends page
	When I enter "name" in the search box
	Then I should see their friend card on the page