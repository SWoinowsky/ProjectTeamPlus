@Seth
Feature: Ability for a user to create a competition

A short summary of the feature

#Okay so I don't want to go through the process of fetching cookies right now so this is assuming the TestUser has a steam id tied to it

# Seriously. These tests will require the TestUser class to have been tied to the NotABogusForClass steam account to pass.

@LoggedIn
Scenario: I am the test user and want to visit the competition creation page
	Given I am signed in
	When I click on the "Compete" link
	And I click on the compete create link
	Then I land on the compete create page