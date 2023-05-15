@Carlos
Feature: Ability to assign a nickname to a friend on the Friends page

A user may want to be able to assign nicknames to their friends on the platform for whatever reason

@LoggedIn
@RevertNickname
Scenario Outline: I am a user with a friend on the Friends page wanting to give them a nickname
	Given I have a friend on the friends page
	And I click on the name on the friend card "<FriendName>"
	When I enter a nickname "<Alias>" for "<FriendName>"
	Then I should see "<FriendName>" referenced as "<Alias>"
Examples: 
| FriendName  | Alias       |
| Steve       | Minecraft   |


@LoggedIn
@SetupAlias
Scenario Outline: I am a user with a friend on the Friends page wanting to remove the nickname I gave them
	Given I have a friend on the friends page
	And "<FriendName>" has a set nickname
	When I click on the revert button for "<SteamId>"
	Then I should see the name of "<FriendName>" return to original
Examples: 
| FriendName | SteamId           |
| Steve      | 76561199093267477 |

