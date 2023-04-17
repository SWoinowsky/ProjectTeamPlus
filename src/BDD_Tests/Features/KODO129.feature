@Seth
Feature: Ability to see a list of competitions a user is assigned to

A short summary of the feature

@tag1
Scenario: I am a user and can visit a page that shows my competitions.
	Given I am signed in
	When I click on the compete link
	Then I end up on the competitions list page

#Okay so I don't want to go through the process of fetching cookies right now so this is assuming the TestUser has a steam id tied to it

# Seriously. These tests will require the TestUser class to have been tied to the NotABogusForClass steam account to pass.