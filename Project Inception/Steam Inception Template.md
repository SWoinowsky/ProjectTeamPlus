Project Inception Worksheet
=====================================

## Summary of Our Approach to Software Development
    Our approach to software development for this project is to utilize Scrum from the Agile software 
    development methodology. 
    
    The project will be broken down into sprints, with each sprint consisting of a planning meeting, 
    daily stand-up meetings, and a retrospective at the end of the sprint. The team will prioritize and 
    select user stories to be completed during each sprint, with the goal of delivering a potentially releasable 
    product increment at the end of each sprint.
    
    The team will also use Scrum ceremonies such as the sprint planning, daily stand-ups, sprint review, and sprint 
    retrospective to continuously improve the development process. Additionally, we will also use agile development 
    tools such as Jira to keep track of progress and manage tasks.

## Initial Vision Discussion with Stakeholders
    App with news tracking for Steam users, will also provide achievement tracking and competition arrangement.

### Description of Clients/Users
    Users of the Steam game library and store, hosted by Valve. Gaming hobbyists interested in achievement hunting and
    in staying up-to-date with their favorite games.

### List of Stakeholders and their Positions (if applicable)
    App Developers: We are responsible for designing and building the app.
    Users: The primary stakeholders, using the app to track news and compete with friends over achievements.
    Game Developers: Relies on information from developers about their games. 
    Game Journalist: Centralized news site with summarization would make it easier to do research and stay up to date. 
    Cloud Provider: Provide infrastructure and resources for the app's operations.

## Initial Requirements Elaboration and Elicitation
[Requirements E & E](../Milestones/Milestone3/Requirements%20E&E.md)

### Elicitation Questions
    1. Can you describe your current method for staying updated on news and updates for the games you follow?
    2. What features do you consider most important in an app that tracks news and updates for games?
    3. How do you currently keep track of your own statistics about the games you play?
    4. Are there any specific challenges or pain points you experience when trying to access news and updates about games?
    5. How important is the ability for devolpers to share and disseminate industry news and updates to you?
    6. How important is a mobile-friendly interface to you?
    7. How important is the ability to receive push notifications?
    10. Are there any other features or functionalities you would like to see in an app like this?

### Technical Questions
    1. Are we able to access steams authentication servers correctly?
        - Does our method support .Net 7?
    2. Do we need a publishers license to get all the required data we need? 
    3. Are we within Steams terms of service for the API usage
    4. What information should be shown on the landing page after the user logs in. 
        - Planning on showing avaible games but there could be additional stats we can show

### Elicitation Interviews
    Transcript or summary of what was learned

### Other Elicitation Activities?
    As needed

## Needs and Features
    Need: A user needs to stay up to date on news for their favorite games
        Feature: A custom dashboard with the latest gaming news tailored to them
        Feature: Receive quick, summarized news regarding their favorite games via SMS
        Feature: A centralized platform much easier to navigate and use compared to current solutions
        Feature: Subscribe to news for games currently owned, or get recommended new games based on your library

    Need: A user needs to see what achievements they have earned for the games in their library
        Feature: A dedicated page that shows the user's achievements earned for each of their games
        Feature: A search bar to find the game they are looking for easier
        Feature: Ability to navigate to a friend's page and see and compare their achievements

    Need: A user needs to compete with other achievement hunters
        Feature: A competitive leaderboard with scheduled races for all sorts of games and popular race categories
        Feature: Ability to join and create teams to work together in competitions and win  
        Feature: Custom badges awarded to individuals and teams to flaunt
        Feature: Special titles that appear on a user's profile earned by winning achievement races
    
    Need: A user needs a trustworthy platform to do all of these
        Feature: A clean looking, modern website to be a part of
        Feature: Steam sanctioned log-in 
        Feature: No private data is saved or required to create an account
        Feature: Detach a user's account and sign in with another Steam account


## Initial Modeling

### Use Case Diagrams
    Diagrams


### Sequence Diagrams

### Other Modeling
#### Mindmap
[![MindMap](../Milestones/Milestone2/Mindmaps/Steam%20Info%20Network%20Mindmap.jpg)](https://github.com/SWoinowsky/ProjectTeamPlus/blob/dev/Milestones/Milestone2/Mindmaps/Steam%20Info%20Network%20Mindmap.jpg)

---

## Identify Non-Functional Requirements 

1. Security: The app must have appropriate security measures in place to protect user data and prevent unauthorized access to sensitive information.
2. Performance: The app must be able to handle a high volume of users and data, and perform well even under heavy load.
3. Scalability: The app should be able to handle an increasing number of users and data without significant changes to the system.
4. Usability: The app should be easy to navigate and use for users of all skill levels.
5. Accessibility: The app should be accessible for users with disabilities, such as screen readers and keyboard navigation.
6. Compatibility: The app should be compatible with a variety of devices, browsers and platforms.
7. Reliability: The app should be reliable and available for use with minimal downtime.
8. Maintainability: The app should be easy to maintain and update, with clear documentation and a user-friendly interface.
9. Compliance: The app should comply with all relevant laws and regulations, such as data protection and privacy laws.
10. Internationalization: The app should support multiple languages and localization.

---

## Identify Functional Requirements (In User Story Format)

E: Epic  
U: User Story  
T: Task  

## [E] User Information Access
    1. [U] As a user, I would like to have all of my Steam account statistics in one organized page.
        a. [T]
        b. [T]
    2. [U] As a user, I would like to have my owned-game library displayed on a page.

## [E] Achievement Tracking
    1. [U] As a user, I would like to have information regarding my in-game achievements displayed on another page.
        a. [T]
    2. [U] As a user, I would like to be able to invite other users to have their achievements recorded alongside mine in an "event"
        a. [T]
    3. [U] As a user, I would like to be able to set a date in which these "events" stop recording.
        a. [T]
    4. [U] As a user, I would like to be able to view relative leaderboard positions for other users participating in events.
        a. [T]
    5. [U] As a user, I would like to be able to publicly display that I have placed in specific events.
        a. [T]
    6. [U] As a user, I would like to have the choice on how an event is completed.
        a. [T]
    7. [U] As a user, I would like to be able to designate teams for these events.
        a. [T]

## [E] Game Curation
    1. [U] As a user, I would like to be able to navigate to a page that displays news about games.
        a. [T]
    2. [U] As a user, I would like to be able to select which games' news and information I want to see.
        a. [T]
    3. [U] As a user, I'd like to be presented with interesting information regarding popular games.
        a. [T]

## [E] SMS Interaction
    1. [U] As a user, I'd like to receive texts when news comes out regarding my favorite games
        a. [T]
    2. [U] As a user, I'd like to receive texts regarding the events I'm participating in.
        a. [T]
    3. [U] As a user, I would like to be able to request information regarding myself via text.
        a. [T]
    4. [U] As a user, I would like to be able to opt out of texts via text.
        a. [T]
---

## Initial Architecture Envisioning
[![Initial Architecture](../Milestones/Milestone2/Technology%20Diagrams/Technology%20Diagram%20-%20Steam.png)](https://github.com/SWoinowsky/ProjectTeamPlus/blob/dev/Milestones/Milestone2/Technology%20Diagrams/Technology%20Diagram%20-%20Steam.png)

### Agile Data Modeling
    Diagrams, SQL modeling (dbdiagram.io), UML diagrams

## Timeline and Release Plan
[Release Schedule](../Milestones/Milestone3/Timeline%20and%20Release%20Plan.html)



    