Feature: Ability for a user to see a list of their Steam friends on their profile page

A short summary of the feature

#Okay so I don't want to go through the process of fetching cookies right now so this is assuming the TestUser has a steam id tied to it

# Seriously. These tests will require the TestUser class to have been tied to the NotABogusForClass steam account to pass.

@tag1
Scenario: I am a user and am looking for the list of friends on my profile page.
	Given I am signed in
	When I click on the profile link
	Then Then I can see my list of friends