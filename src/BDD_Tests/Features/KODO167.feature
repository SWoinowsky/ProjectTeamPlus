@Carlos

Feature: Ability to send an email to someone on the Friends page

A user may want to be able to send an invitation to someone to friends 

@LoggedIn
Scenario: I am a user wanting to send an email invitation to someone so they can join S.I.N
	Given I have a friend on the friends page
	And I click on the envelope icon
	When I enter a valid email "address"
	Then I should see a success message and the modal has closed