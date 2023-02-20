function setHiddenGame(name)
{
    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('value');
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/api/Steam/hide",
        data: JSON.stringify(gameId),
        success: hideGame(gameName),
        error: errorOnAjax
    })
}

function hideGame(gameName)
{
    gameName.style.display = "none";
    gameName.hidden = true;
}

function errorOnAjax() {
    console.log("ERROR on ajax request");
}

