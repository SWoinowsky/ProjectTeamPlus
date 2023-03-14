function setFollowedGame(name)
{
    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('value');
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/follow?id=${gameId}`,
        success: followGame(name, gameId),
        error: errorOnAjax
    })
}

function setUnfollowGame(name)
{
    var gameName = document.getElementById(name);
    var gameId = gameName.getAttribute('value');
    $.ajax({
        type: "POST",
        dataType: "text",
        url: `/api/Steam/unfollow?id=${gameId}`,
        success: unfollowGame(name, gameId),
        error: errorOnAjax
    })
}

function unfollowGame(gameName, gameId)
{
    console.log(`${gameName.name} has been unfollowed successfully`);
}
 
function followGame(gameName, gameId) {
    console.log(`${gameName} has been followed/unfollowed successfully`);

    var followIcon = document.getElementById(`${gameId} followicon`);
    $(followIcon).toggleClass('bi bi-bookmark-dash-fill');
    $(followIcon).toggleClass('bi bi-bookmark-plus');
    $(followIcon).attr('color', 'green');

}

function errorOnAjax() {
    console.log("ERROR on ajax request");
}

