

var gameAchievements;
var userAchievements;
var friendAchievements;

$( function () {
    
    modalActivations();

    ajaxFetchGames();

    setInterval( ajaxFetchGames, 5000 );
});

function errorOnAjaxGames() {
    console.log( "ERROR in ajax games request" );
}

function errorOnAjaxAchievements() {
    console.log( "ERROR in ajax achievements request" );
}

function errorOnAjaxSchema() {
    console.log( "ERROR in ajax schema request" );
}

function updateFriendStatus( data )
{
    var states = ["offline", "online", "busy", "away", "snooze"];
    var state =  data.personaState;
    
    if( state == null )
    {
        state = 0;
    }

    var statusElement = document.getElementById("friendPageStatus");
    statusElement.innerHTML = states[state];

    if( data.gameExtraInfo != null )
    {
        var gameElement = document.getElementById("friendGameStatus");
        gameElement.innerHTML = `Playing ${data.gameExtraInfo}`;
    }
}

function ajaxFetchGames() {
    var SteamId = document.getElementById("SteamId").value;
    var FriendSteamId = document.getElementById("FriendSteamId").value;
    var Id = document.getElementById("Id").value;


    $.ajax({
        type: "GET",
        dataType: "json",
        url: `/api/Steam/friendSpecific?userSteamId=${SteamId}&UserId=${Id}&friendSteamId=${FriendSteamId}`,
        success: updateFriendStatus,
        error: errorOnAjaxGames
    });
}

function storeUserAchievements( data ) {
    var clickedGameId = document.getElementById("ClickedGame");

    userAchievements = data.playerstats.achievements;

    processData(clickedGameId.value);
}

function ajaxFetchUserAchievements(id) {
    var SteamId = document.getElementById("SteamId").value;

    $.ajax({
        type: "GET",
        datatype: "json",
        url: `/api/Steam/achievements?steamid=${SteamId}&appId=${id}`,
        success: storeUserAchievements,
        error: errorOnAjaxAchievements
    });
}

function storeFriendAchievements( data ) {
    var clickedGameId = document.getElementById("ClickedGame");

    friendAchievements = data.playerstats.achievements;

    ajaxFetchUserAchievements(clickedGameId.value);
}

function ajaxFetchFriendAchievements(id) {
    var FriendSteamId = document.getElementById("FriendSteamId").value;

    $.ajax({
        type: "GET",
        datatype: "json",
        url: `/api/Steam/achievements?steamid=${FriendSteamId}&appId=${id}`,
        success: storeFriendAchievements,
        error: errorOnAjaxAchievements
    });
}

function storeSchema( data ) {
    var noAchErr = document.getElementById("error-ach");
    var noResErr = document.getElementById("error-ser");
    var spinner = document.getElementById("loading-spinner");
    var clickedGameId = document.getElementById("ClickedGame");

    if( data == null )
    {
        spinner.style.display = "none";
        noResErr.style.display = "flex";
    }
    else if( data.game.availableGameStats == null ) {
        spinner.style.display = "none";
        noAchErr.style.display = "flex";
    }
    else {
        gameAchievements = data.game.availableGameStats.achievements;

        ajaxFetchFriendAchievements(clickedGameId.value);
    }
}

function ajaxFetchSchema(appId) {
    $.ajax({
        type: "GET",
        datatype: "json",
        url: `/api/Steam/schema?appId=${appId}`,
        success: storeSchema,
        error: errorOnAjaxSchema
    });

    var gameImage = document.getElementById("gameImage");
    gameImage.src = `https://steamcdn-a.akamaihd.net/steam/apps/${appId}/library_600x900_2x.jpg`;

}

function modalActivations() {
    var cards = document.getElementsByClassName("friendDiv");
    var clickedGameId = document.getElementById("ClickedGame");
    var spinner = document.getElementById("loading-spinner");
    var body = document.getElementsByTagName("body");
    var closeBtn = document.getElementById("modalCloser");
    var noAchErr = document.getElementById("error-ach");
    var noResErr = document.getElementById("error-ser");
    
        
    closeBtn.onclick = function() {
        $("#compareModal").modal("hide");
    }

    for( card of cards ) {
        card.onclick = function () {
            var infoDisplay = document.getElementById("informationRow");
            var competeButton = document.getElementById("competeBtn");
            var FriendSteamId = document.getElementById("FriendSteamId").value;
            var appId = this.id;

            competeButton.onclick = function() {
                location.href = `/compete/${FriendSteamId}/${appId}`;
            }

            spinner.style.display = "flex";
            infoDisplay.style.display = "none";
            noAchErr.style.display = "none";
            noResErr.style.display = "none";

            console.log(`clicked on ${this.id}`);
            $("#compareModal").modal("toggle");
            clickedGameId.value = this.id;

            ajaxFetchSchema(this.id);
        }
    }
}

function processData(appId)
{
    var spinner = document.getElementById("loading-spinner");
    var infoDisplay = document.getElementById("informationRow");
    var userAchList = document.getElementById("userAchievements");
    var friendAchList = document.getElementById("friendAchievements")
    
    gameAchievements.sort((a, b) => a.name < b.name );


    userAchList.innerHTML = "<i>Your achievements</i>";
    friendAchList.innerHTML = "<i>Their achievements</i>";
    for( achievement of gameAchievements )
    {
        for( userAch of userAchievements )
        {
            let achIcon = document.createElement("div");
            achIcon.className = "achievementItem";

            if( userAch.apiname == achievement.name && userAch.achieved == 1 )
            {
                achIcon.innerHTML =     `<div class="ach-name">${achievement.displayName}</div>
                                        <img class="ach-img" src=${achievement.icon}>`;
                userAchList.append(achIcon);

                console.log(`Added achievement ${achievement.displayName} with status 'earned' for user. `)

            }
            else if ( userAch.apiname == achievement.name && userAch.achieved == 0 )
            {
                achIcon.innerHTML =     `<div class="ach-name">${achievement.displayName}</div>
                                        <img class="ach-img" src=${achievement.icongray}>`;

                userAchList.append(achIcon);

                console.log(`Added achievement ${achievement.displayName} with status 'unearned' for user.`)

            }
        }

    
        
        for( friendAch of friendAchievements )
        {
            let achIcon = document.createElement("div");
            achIcon.className = "achievementItem";

            if( friendAch.apiname == achievement.name && friendAch.achieved == 1 )
            {
                achIcon.innerHTML =     `<div class="ach-name">${achievement.displayName}</div>
                                        <img class="ach-img" src=${achievement.icon}>`;
                friendAchList.append(achIcon);

                console.log(`Added achievement ${achievement.displayName} with status 'earned' for friend.`)
            }
            else if ( friendAch.apiname == achievement.name && friendAch.achieved == 0 )
            {
                achIcon.innerHTML =     `<div class="ach-name">${achievement.displayName}</div>
                                        <img class="ach-img" src=${achievement.icongray}>`;
                                        
                friendAchList.append(achIcon);

                console.log(`Added achievement ${achievement.displayName} with status 'unearned' for friend.`)

            }
        }
    }

    spinner.style.display = "none";
    infoDisplay.style.display = "flex";
}