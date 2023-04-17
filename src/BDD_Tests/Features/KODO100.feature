@Seth
Feature: Ability for a user to see shared games between them and a friend

A short summary of the feature

#Okay so I don't want to go through the process of fetching cookies right now so this is assuming the TestUser has a steam id tied to it

# Seriously. These tests will require the TestUser class to have been tied to the NotABogusForClass steam account to pass.

@tag1
Scenario: I am the test user and want to see games I share with Eithné of Brokiloén
	Given I am signed in
	When I click on the profile link
	And I click on Eithné's friend page link
	Then I can see the shared games page for Eithné
	And I can see Eithné's username