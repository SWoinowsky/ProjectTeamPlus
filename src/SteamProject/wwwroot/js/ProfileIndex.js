$( function () {
    ajaxFetch();

    setInterval( ajaxFetch, 5000);
});

function errorOnAjax() {
    console.log( "ERROR in ajax request" );
}

function updateFriendStatuses( data ) {
    var Statuses = document.getElementsByClassName("FriendStateTd");
    var statusArr = Array.prototype.slice.call(Statuses);
    var Games = document.getElementsByClassName("FriendGame");
    var gamesArr = Array.prototype.slice.call(Games);

    var table = document.getElementById("friendsListTable");
    table.innerHTML = "";
    data.sort((a, b) => a.steamName.localeCompare(b.steamName) > 0);

    var tdList = [];

    $.each( data, function ( index, item ) {
        var game = item.gameExtraInfo;
        if( game == null ) {
            game = "";
        }

        var state = item.personaState;
        if( state == null ) {
            state = 0;
        }

        var states = ["offline", "online", "busy", "away", "snooze"];

        var entry = document.createElement("tr");
        entry.className = "FriendEntry";
        entry.id = item.steamName;
        entry.innerHTML = 
        `
            <td> <img src=${item.avatarUrl} > </td>
            <td> ${item.steamName} </td>
            <td class="FriendStateTd"> <i> ${states[state]} </i> </td>
            <td class="FriendGame"> ${game} </td>
        `
        ;

        table.append(entry);
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