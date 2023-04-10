function typeWriter(element, text, i, callback) {
    if (i === 0) {
        element.innerHTML = "";
    }

    if (i < text.length) {
        element.innerHTML += text.charAt(i);
        setTimeout(() => {
            typeWriter(element, text, i + 1, callback);
        }, 5);
    } else {
        element.insertAdjacentHTML("afterend", "</div>");
        if (callback) {
            callback();
        }
    }
}

function saveGameNewsToLocalStorage(appId, summarizedNews, timestamp) {
    localStorage.setItem(`gameNews-${appId}`, summarizedNews);
    localStorage.setItem(`gameNewsTimestamp-${appId}`, timestamp);
}

function isGameNewsExpired(storedNewsTimestamp) {
    const now = new Date().getTime();
    const expirationTime = 60 * 60 * 1000;
    return !storedNewsTimestamp || now - storedNewsTimestamp > expirationTime;
}

function fetchGameNewsFromAPI(appId, gameType) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/api/News/GetRecentGameNews",
            data: { appId: appId },
            success: function (response) {
                console.log(response);
                const appId = response.appId;
                const summarizedNews = response.summarizedNews;
                const now = new Date().getTime();

                saveGameNewsToLocalStorage(appId, summarizedNews, now);
                resolve({ appId, summarizedNews, gameType, isFromAPI: true });
            },
            error: function (error) {
                reject(error);
            }
        });
    });
}

function getRecentGameNews(appId, gameType) {
    const storedNews = localStorage.getItem(`gameNews-${appId}`);
    const storedNewsTimestamp = localStorage.getItem(`gameNewsTimestamp-${appId}`);

    if (storedNews && !isGameNewsExpired(storedNewsTimestamp)) {
        return Promise.resolve({ appId, summarizedNews: storedNews, gameType, isFromAPI: false });
    } else {
        return fetchGameNewsFromAPI(appId, gameType);
    }
}

function updateGameNews({ appId, summarizedNews, gameType, isFromAPI }) {
    const gameElement = document.querySelector(`.card .card-body .${gameType}-game-news[data-gameid="${appId}"]`);

    if (isFromAPI) {
        typeWriter(gameElement, summarizedNews, 0);
    } else {
        gameElement.innerHTML = summarizedNews;
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const recentGames = document.querySelectorAll(".recent-game-news");
    const followedGames = document.querySelectorAll(".followed-game-news");

    recentGames.forEach((game) => {
        const appId = game.getAttribute("data-gameid");
        getRecentGameNews(appId, "recent")
            .then(updateGameNews)
            .catch((error) => console.error(error));
    });

    followedGames.forEach((game) => {
        const appId = game.getAttribute("data-gameid");
        getRecentGameNews(appId, "followed")
            .then(updateGameNews)
            .catch((error) => console.error(error));
    });
});
