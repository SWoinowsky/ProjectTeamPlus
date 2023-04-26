$( function () {
    showCurrentIndex();
    checkSelect();
});

function findForm() {
    return document.getElementById("formInternals")
};

function showCurrentIndex() {
    categories = ["Duel", "Free-For-All"];

    var selector = document.getElementById("categorySelect");
    var index = selector.selectedIndex;
    var text = selector.children[index].value;
    
    console.log(text);
    if( text == categories[0] )
    {
        showDuel();
    }
    else if( text == categories[1] )
    {
        showFFA();
    }
}

function checkSelect() {
    var selector = document.getElementById("categorySelect");

    selector.onchange = function() {
        showCurrentIndex();
    }
}

function showFFA() {
    var pageForm = findForm();
    pageForm.innerHTML = "";

    var eleNew = document.createElement('div');
    eleNew.className = "DynamicInput";
    eleNew.innerHTML = "THIS IS THE FREE FOR ALL RESULT";


    pageForm.append(eleNew);
}

function showDuel() {
    var pageForm = findForm();
    pageForm.innerHTML = "";

    var DuelDiv = document.createElement('div');
    DuelDiv.className = "DynamicInput";
    DuelDiv.id = "DuelDiv";
    DuelDiv.innerHTML = "THIS IS THE DUEL RESULT <br>";

    getFriendsListForDuel();

    pageForm.append(DuelDiv);
}

function singleFriendSelect( data ) {
    var duelDiv = document.getElementById("DuelDiv");

    var friendSelect = document.createElement("select");
    friendSelect.id = "friendSelector"
    friendSelect.name = "OpponentId";
    $.each( data, function ( index, item ) {
        var option = document.createElement("option");
        option.value = `${item.steamId}`;
        option.innerHTML = `${item.steamName}`

        friendSelect.append( option );
    });

    friendSelect.onchange = function() {
        getGamesForDuel();
    }

    duelDiv.append( friendSelect );

    getGamesForDuel();
}

function errorOnAjax() {
    console.log( "ERROR in ajax request" );
}

function getFriendsListForDuel() {
    var SteamId = document.getElementById("SteamId").value;
    var SinId = document.getElementById("SinId").value;

    $.ajax({
        type: "GET",
        dataType: "json",
        url: `/api/Steam/friends?steamid=${SteamId}&UserId=${SinId}`,
        success: singleFriendSelect,
        error: errorOnAjax
    });
}

function addGameSelector( data ) {
    var GameDivPrevious = document.getElementById("GameDiv");
    if( GameDivPrevious != null ) {
        GameDivPrevious.remove();
    }

    var GameDiv = document.createElement('div');
    GameDiv.id = "GameDiv";
    GameDiv.innerHTML = "";


    var gameSelector = document.createElement("select");
    gameSelector.name = "GameAppId";

    $.each( data, function ( index, item ) {
        var option = document.createElement("option");
        option.value = `${item.appId}`;
        option.innerHTML = `${item.name}`

        gameSelector.append( option );
    });

    GameDiv.append( gameSelector );

    var DuelDiv = document.getElementById("DuelDiv");
    DuelDiv.append( GameDiv );
}

function getGamesForDuel() {
    var SteamId = document.getElementById("SteamId").value;
    var SinId = document.getElementById("SinId").value;

    var friendSelector = document.getElementById("friendSelector");
    var index = friendSelector.selectedIndex;
    var friendId = friendSelector.children[index].value;

    $.ajax({
        type: "GET",
        dataType: "json",
        url: `http://localhost:5105/api/Steam/sharedGames?userSteamId=${SteamId}&friendSteamId=${friendId}&userId=${SinId}`,
        success: addGameSelector,
        error: errorOnAjax
    });
}

function getAchievementsForDuel() {
    
}