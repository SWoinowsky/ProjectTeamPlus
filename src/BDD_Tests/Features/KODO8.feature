@Cole
Feature: Ability for a user to see icons and information to prove they're logged in with Steam.

A short summary of the feature

#Okay so I don't want to go through the process of fetching cookies right now so this is assuming the TestUser has a steam id tied to it

# Seriously. These tests will require the TestUser class to have been tied to the NotABogusForClass steam account to pass.

@tag1
Scenario: I am a user and need to see that my steam account is connected.
	Given I am signed in
	When I click on the profile link
	Then I can see my Steam profile image
	And I can see my Steam username and level