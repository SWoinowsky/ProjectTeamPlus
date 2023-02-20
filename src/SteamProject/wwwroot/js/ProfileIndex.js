$( function () {
    ajaxFetch();

    setInterval( ajaxFetch, 60000);
});

function errorOnAjax() {
    console.log( "ERROR in ajax request" );
}

function updateFriendStatuses( data ) {
    var Statuses = document.getElementsByClassName("FriendStateTd");
    var Games = document.getElementsByClassName("FriendGame");

    var i = 0;
    $.each( data, function ( index, item ) {
        var game = item.gameExtraInfo;
        if( game == null ) {
            game = "";
        }

        var state = item.personaState;
        var states = ["offline", "online", "busy", "away", "snooze"];
        Statuses[i].innerHTML = ` <i> ${states[state]} </i>`;
        Games[i].innerHTML = `${game}`;
        i = i + 1;
    });
}

function ajaxFetch() {
    var SteamId = document.getElementById("SteamId").value;
    var Id = document.getElementById("Id").value;


    $.ajax({
        type: "GET",
        dataType: "json",
        url: `/api/Steam/friends?steamid=${SteamId}&UserId=${Id}`,
        success: updateFriendStatuses,
        error: errorOnAjax
    });
}