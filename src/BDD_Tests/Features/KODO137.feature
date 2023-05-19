@Cole
Feature: BadgeAwardSystem

As a user, I want to be rewarded for my activities on the platform, So that I can feel a sense of achievement and have a more enjoyable experience.


@Badge
Scenario: I am a test user and I want to make sure my Dashboard page contains the "Nexus Newcomer" badge after I have linked my Steam account
	Given I am signed in
	When I click on the "Dashboard" link
	Then I end up on the "Dashboard" page
	And I should see a badge named "Nexus Newcomer"
	When I click on the badge named "Nexus Newcomer"
	Then I should see a detailed description of the badge "Nexus Newcomer"

