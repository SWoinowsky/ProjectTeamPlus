Project Inception Worksheet
=====================================

## Summary of Our Approach to Software Development
    Agile, using git workflow described by Scot.

## Initial Vision Discussion with Stakeholders
    App with news tracking for Steam users, will also provide achievement tracking and competition arrangement.

### Description of Clients/Users
    Users of the Steam game library and store, hosted by Valve. Gaming hobbyists interested in achievement hunting and
    in staying up-to-date with their favourite games.

### List of Stakeholders and their Positions (if applicable)
    Stakeholders are currently only the developers. Document will be updated to reflect any changes.

## Initial Requirements Elaboration and Elicitation
    See Requirements_template for more

### Elicitation Questions
    1. 
    2.
    3. ...

### Elicitation Interviews
    Transcript or summary of what was learned

### Other Elicitation Activities?
    As needed

## List of Needs and Features
    1. Access to Steam Web API, already collected and tested using CURL. Further implementation / demonstration is doable.
    2. Access to Twilio SMS API. Strong need to sign up and test this.
    3. Access to OpenID API. Also strong need to figure out the implementation here.
    4. Connection / Integration with SQL Server DB. For purposes of tracking users' activity on website.

## Initial Modeling

### Use Case Diagrams
    Diagrams

### Sequence Diagrams

### Other Modeling
    Diagrams, UI wireframes, page flows, ...

## Identify Non-Functional Requirements
    1.
    2.
    3.

## Identify Functional Requirements (In User Story Format)

E: Epic  
U: User Story  
T: Task  

3. [E] User Information Access
    1. [U] As a user, I would like to have all of my Steam account statistics in one organized page.
        a. [T]
        b. [T]
    2. [U] As a user, I would like to have my owned-game library displayed on a page.

11. [E] Achievement Tracking
    4. [U] As a user, I would like to have information regarding my in-game achievements displayed on another page.
        a. [T]
    5. [U] As a user, I would like to be able to initiate custom events that capture user achievements within a timespan.
        a. [T]
    6. [U] As a user, I would like to be able to invite other users to have their achievements recorded alongside mine.
        a. [T]
    7. [U] As a user, I would like to be able to view relative leaderboard positions for other users participating in events.
        a. [T]
    8. [U] As a user, I would like to be able to publicly display that I have placed in specific events.
        a. [T]
    9. [U] As a user, I would like to have the choice on how an event is completed.
        a. [T]
    10. [U] As a user, I would like to be able to designate teams for these events.
        a. [T]

15. [E] Game Curation
    12. [U] As a user, I would like to be able to navigate to a page that displays news about games.
        a. [T]
    13. [U] As a user, I would like to be able to select which games' news and information I want to see.
        a. [T]
    14. [U] As a user, I'd like to be presented with interesting information regarding popular games.
        a. [T]

20. [E] SMS Interaction
    16. [U] As a user, I'd like to receive texts when news comes out regarding my favourite games
        a. [T]
    17. [U] As a user, I'd like to receive texts regarding the events I'm participating in.
        a. [T]
    18. [U] As a user, I would like to be able to request information regarding myself via text.
        a. [T]
    19. [U] As a user, I would like to be able to opt out of texts via text.
        a. [T]

## Initial Architecture Envisioning
    Diagrams and drawings, lists of components

## Agile Data Modeling
    Diagrams, SQL modeling (dbdiagram.io), UML diagrams

## Timeline and Release Plan
    <!-- Schedule: meaningful dates, milestones, sprint cadence, how releases are made (CI/CD, or fixed releases, or ?) -->
    Needs completed, pushed, and published before June 2023.
    