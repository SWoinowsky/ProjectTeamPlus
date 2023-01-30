# Requirements Workup

## Top Priority Feature 1: User Information Access 

### Goal: 
#### The goal for this feature is to allow a user to be able to log into their Steam account securely through Steams authentication servers to retrieve information such as their SteamID, game library, friends list and whatever other information we need to access through the steamAPI. We will need to parse the json coming from steamAPI into usable data models and store it for later use. Once the user is authenticated, they should be able to see what games they own in their library along with other additional details we feel are important to the user. 

## Top Priority Feature 2: Game Curation 

### Goal: 
#### The goal for this feature is to allow a user to navigate a list of their owned steam games and view any news or updates for said games. It should also allow the user to pick and choose which games they would like to receive updates on to give them the ability to filter out games based on the users' preference. In addition, the user should also be able to access interesting information about the game that they are examining.


---

## Elicitation for Feature 1:

1. Is the goal or outcome well-defined?  Does it make sense?
    * I believe so as the goal for this feature is mainly to access Steams API securely, and it makes sense as a good starting point for the rest of the project that relies on Steam's data.
2. What is not clear from the given description?
    * Other details besides the game library to show the user needs to be expanded on
3. How about scope?  Is it clear what is included and what isn't?
    * Scope for this feature center around API access to Steam servers through secure authentication of the user
    * Initial scope is defined clearly around displaying a users game library to start with. 
4. What do you not understand?
    * Technical domain knowledge
        * Access to steam authentication servers through OpenID is confusing and outdated.
            * Does it support .net 7?
        * Do we need a publishers license to get all required data?
    * Business domain knowledge
        * Are we within steams APIs terms of service for what we are doing?
5. Is there something missing?
    * A landing page for after they sign in and before they look at their library maybe?
6. Get answers to these questions.
    * Tested aspnet.security.openID to allow access to Steam authentication servers, and it works and returns what it should and also supports .net 7
    * Still unsure of if we need a publishers licenses, but we learned we did not need it yet.
    * If we follow the guidelines posted [here](https://steamcommunity.com/dev/apiterms) we should be good.


## Elicitation for Feature 2:

1. Is the goal or outcome well-defined?  Does it make sense?
    * The goal is to allow a user to view what games they own and examine said games, I believe that is a clear and well-defined goal that does make sense for the scope of our project as we will need this step for the next big feature which has to do with achievements. 
2. What is not clear from the given description?
    * We need to decide what interesting information we need to display about each game?
    * How exactly we will display this information?
    * Will additional stats be on the same page, or will it redirect you to a new page?
3. How about scope?  Is it clear what is included and what isn't?
    * Scope around displaying their games library is self-contained, adding additional details to be displayed could be separated off into its own scope if it increases in scale and complexity depending on what all we display. 
4. What do you not understand?
    * Technical domain knowledge
        * How much information can we display about every game?
            - Depends on what data the API give us
                - Would a publishers license give us access to more valuable information?
        * What information should we display?
    * Business domain knowledge
        * What information is available to us from each game developer, and do they all follow the same guidelines?
        - Does news and updates look the same from every source?
5. Is there something missing?
6. Get answers to these questions.



## Analysis

### User Information Access
#### Bounds, Limitations, Types, and Constraints

   + The main purpose of this feature is to securely log into a Steam account using Steam's authentication servers to retrieve user information such as SteamID, game library, friends list, etc. through the Steam API.
   + The initial scope is limited to displaying a user's game library and storing the information for later use.
   + The data must be parsed from the json returned by the Steam API and stored in usable data models.
   + Technical limitations include the need to understand access to Steam's authentication servers through OpenID and determine if a publisher's license is necessary to get all the required data.
   + Business constraints include adhering to Steam's API terms of service and understanding the information that is available from each game developer.

Conflicts

    None detected so far.
.
Missing Information

   + A landing page for after authentication and before viewing the game library is missing from the given information.

### Game Curation

#### Bounds, Limitations, Types, and Constraints
   + The main purpose of this feature is to allow a user to navigate their owned Steam games, view news or updates, pick and choose games to receive updates on, and access interesting information about the games.
   + The scope is limited to displaying a user's game library but additional details could be separated off into their own scope if they become complex.
   + Technical limitations include understanding what information can be displayed about each game, the amount of information available, and the need for a publisher's license to access valuable information.
   + Business constraints include understanding what information is available from each game developer and if they all follow the same guidelines, and if news and updates look the same from every source.

### Conflicts

    None detected so far.

### Missing Information

+ Information about what interesting information to display about each game and how to display it is missing.
+ The format of news and updates from different sources is not clear.

## Design and Modeling
[![ER Diagram](./../Milestone3/Data%20Model/Initial%20Data%20Model.png)](https://github.com/SWoinowsky/ProjectTeamPlus/blob/dev/Milestones/Milestone3/Data%20Model/Initial%20Data%20Model.png)


## Analysis of the Design
The next step is to determine how well this design meets the requirements _and_ fits into the existing system.

1. Does it support all requirements/features/behaviors?
    * For each requirement, go through the steps to fulfill it.  Can it be done?  Correctly?  Easily?
2. Does it meet all non-functional requirements?
    * May need to look up specifications of systems, components, etc. to evaluate this.

