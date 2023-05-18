@Justin
Feature: View Hidden Game Modal

The hidden modal needs to be capable of showing, even if it's empty so a user knows they
have no hidden games.

Scenario: View a hidden modal with no games in it shows a message
	Given I am signed in
	When I click on the "Library" link
	And I click on the hidden modal button
	Then I should see the empty hidden modal
