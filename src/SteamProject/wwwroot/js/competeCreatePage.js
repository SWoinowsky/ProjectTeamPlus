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


    var eleDuel = document.createElement('div');
    eleDuel.className = "DynamicInput";
    eleDuel.id = "DuelDiv"
    eleDuel.innerHTML = "THIS IS THE DUEL RESULT";

    getFriendsListForDuel();


    pageForm.append(eleDuel);
}

function singleFriendSelect( data ) {
    var duelDiv = document.getElementById("DuelDiv");

    var friendSelect = document.createElement("select");
    friendSelect.name = "OpponentId";
    $.each( data, function ( index, item ) {
        var option = document.createElement("option");
        option.value = `${item.steamId}`;
        option.innerHTML = `${item.steamName}`

        friendSelect.append( option );
    });

    duelDiv.append( friendSelect );
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

function getGamesForDuel() {
    var SteamId = document
}
