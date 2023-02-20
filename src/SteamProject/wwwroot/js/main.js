function hideGame(name)
{
    var x = document.getElementById(name);
    console.log("Got the game div to be hidden:" + x);
    x.style.display = "none";
    x.hidden = true;
}

