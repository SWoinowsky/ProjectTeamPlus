document.addEventListener("DOMContentLoaded", function () {
    const recentGames = document.querySelectorAll(".recent-game-news");
    const followedGames = document.querySelectorAll(".followed-game-news");

    recentGames.forEach((game) => {
        const appId = game.getAttribute("data-gameid");
        getGameNews(appId);
    });

    followedGames.forEach((game) => {
        const appId = game.getAttribute("data-gameid");
        getGameNews(appId);
    });
});

function typeWriter(element, text, i) {
    return new Promise((resolve) => {
        function type() {
            if (i < text.length) {
                element.innerHTML += text.charAt(i);
                setTimeout(() => {
                    typeWriter(element, text, i + 1).then(resolve);
                }, 50);
            } else {
                resolve();
            }
        }
        type();
    });
}

async function updateGameNews(appId, summarizedNews) {
    const recentGameElement = document.querySelector(`.recent-game-news[data-gameid="${appId}"]`);
    const followedGameElement = document.querySelector(`.followed-game-news[data-gameid="${appId}"]`);

    localStorage.setItem(`gameNews-${appId}`, summarizedNews);

    if (recentGameElement) {
        await typeWriter(recentGameElement, summarizedNews, 0);
    }
    else {
        await typeWriter(followedGameElement, summarizedNews, 0);
    }
}



function getGameNews(appId) {
    const storedNews = localStorage.getItem(`gameNews-${appId}`);

    if (storedNews) {
        updateGameNews(appId, storedNews);
    } else {
        $.ajax({
            url: "https://localhost:7123/api/YourControllerName/GetGameNews",
            data: { appId: appId },
            success: function (response) {
                console.log(response); // Check the received data

                const appId = response.appId;
                const summarizedNews = response.summarizedNews;

                updateGameNews(appId, summarizedNews);
            }
        });
    }
}
