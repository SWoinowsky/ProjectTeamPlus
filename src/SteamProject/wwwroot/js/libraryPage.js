function setHiddenGame(name, userId)
{
    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('name');
    console.log("Hid " + name);
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/hide?id=${gameId}&userId=${userId}`,
        success: hideGame(gameName),
        error: errorOnAjax
    })
}

function setUnhideGame(name, userId)
{
    console.log(userId);
    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('value');
    console.log("Unhid " + name);
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/unhide?id=${gameId}&userId=${userId}`,
        success: hideGame(gameName),
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

function closeModal()
{
    window.location.reload()
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

