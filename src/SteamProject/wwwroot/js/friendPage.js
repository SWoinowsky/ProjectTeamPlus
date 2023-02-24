$( function () {
    ajaxFetch();

    setInterval( ajaxFetch, 5000 );
});

function errorOnAjax() {
    console.log( "ERROR in ajax request" );
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
}

function ajaxFetch() {
    var SteamId = document.getElementById("SteamId").value;
    var FriendSteamId = document.getElementById("FriendSteamId").value;
    var Id = document.getElementById("Id").value;


    $.ajax({
        type: "GET",
        dataType: "json",
        url: `/api/Steam/friendSpecific?userSteamId=${SteamId}&UserId=${Id}&friendSteamId=${FriendSteamId}`,
        success: updateFriendStatus,
        error: errorOnAjax
    });
}