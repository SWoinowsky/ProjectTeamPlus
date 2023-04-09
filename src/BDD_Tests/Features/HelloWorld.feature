Feature: HelloWorld

A short summary of the feature

@tag1
Scenario: Home page title contains the word "Home"
	Given I am a visitor
	When I am on the "Home" page
	Then The page title contains the word "Home"
