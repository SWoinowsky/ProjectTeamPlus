function setFollowedGame(name)
{

    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('value');
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/follow?id=${gameId}`,
        success: followGame(gameName, gameId),
        error: errorOnAjax
    })
}
 
function followGame(gameName, gameId) {
    console.log(`${gameName.name} has been followed successfully`);

    var followIcon = document.getElementById(`${gameId} followicon`);
    $(followIcon).toggleClass('bi bi-bookmark-dash-fill');
    $(followIcon).toggleClass('bi bi-bookmark-plus');
    $(followIcon).attr('color', 'green');

}
     

function showHiddenGamesModal()
{
    var modal = document.getElementById("hidden-game-table");
    $("#hidden-game-modal").modal("show");
    console.log("Modal was shown");
}

function errorOnAjax() {
    console.log("ERROR on ajax request");
}

