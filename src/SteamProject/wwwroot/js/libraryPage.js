function setHiddenGame(name)
{
    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('value');
    console.log("Hid " + name);
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/hide?id=${gameId}`,
        success: hideGame(gameName),
        error: errorOnAjax
    })
}

function setUnhideGame(name)
{
    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('value');
    console.log("Unhid " + name);
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/unhide?id=${gameId}`,
        success: hideGame(gameName),
        error: errorOnAjax
    })
}

function showMoreInfo(id)
{
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/info?id=${id}`,
        success: console.log("Sending user to game info page."),
        error: errorOnAjax
    })
}

function refreshLibrary()
{
    $.ajax({
        type: "GET",
        dataType: "json",
        url: `/api/Steam/refresh`,
        success: window.location.reload(),
        error: errorOnAjax
    })
}

function hideGame(gameName)
{
    gameName.style.display = "none";
    gameName.hidden = true;
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

