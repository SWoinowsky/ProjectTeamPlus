Feature: Ability to see a list of competitions a user is assigned to

A short summary of the feature

@tag1
Scenario: I am a user and can visit a page that shows my competitions.
	Given I am signed in
	When I click on the compete link
	Then The page title should contain "My Competitions"
