function setHiddenGame(name)
{
    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('value');
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/hide?id=${gameId}`,
        success: hideGame(gameName),
        error: errorOnAjax
    })
}
 
function hideGame(gameName)
{
    gameName.style.display = "none";
    gameName.hidden = true;
}

function getAllGames(userId)
{
    $.ajax({
        type: "GET",
        dataType: "text",
        url: `/api/Steam/games?id=${userId}`,
        success: showHiddenGames,
        error: errorOnAjax
    })
}

function showHiddenGames(data)
{
    console.log(data);
    var modal = document.getElementById("hidden-game-table");
    $("#hidden-game-modal").modal("show");
}

function errorOnAjax() {
    console.log("ERROR on ajax request");
}

