let isTyping = {};
let typingStatus = {};

function fadeIn(element) {
    element.classList.add('fade-in');
    element.style.opacity = 1;
    setTimeout(() => {
        element.classList.remove('fade-in');
    }, 500);
}

function fadeOut(element, callback) {
    element.classList.add('fade-out');
    element.style.opacity = 0;
    setTimeout(() => {
        element.classList.remove('fade-out');
        if (callback) {
            callback();
        }
    }, 500);
}

function startLoadingAnimation(element) {
    element.setAttribute("data-original-content", element.innerText);
    const spinnerDiv = document.createElement("div");
    spinnerDiv.classList.add("loading-spinner");
    spinnerDiv.style.display = "flex";
    spinnerDiv.style.justifyContent = "center";
    spinnerDiv.style.alignItems = "center";
    spinnerDiv.style.height = "100%";

    const spinner = document.createElement("div");
    spinner.setAttribute("role", "status");

    spinnerDiv.appendChild(spinner);
    element.innerHTML = "";
    element.appendChild(spinnerDiv);
}

function stopLoadingAnimation(element) {
    const spinnerDiv = element.querySelector(".loading-spinner");
    if (spinnerDiv) {
        element.removeChild(spinnerDiv);
    }
}

function normalizeWhitespace(str) {
    return str.replace(/\s+/g, ' ').trim();
}

function summarizeSpecificNews(button, appId, newsIndex) {
    if (typingStatus[newsIndex]) {
        return;
    }

    const newsElement = button.parentElement.nextElementSibling;
    const originalContent = newsElement.getAttribute("data-original-content");

    let currentNews = normalizeWhitespace(newsElement.innerText);
    let oldNews = normalizeWhitespace(originalContent);

    if (currentNews === oldNews) {
        fadeOut(newsElement, () => {
            const now = new Date().getTime();
            const expirationTime = 60 * 60 * 1000;
            const storedNewsKey = `specificNews-${appId}-${newsIndex}`;
            const storedNewsTimestampKey = `specificNewsTimestamp-${appId}-${newsIndex}`;

            const storedNews = localStorage.getItem(storedNewsKey);
            const storedNewsTimestamp = localStorage.getItem(storedNewsTimestampKey);

            const isExpired = !storedNewsTimestamp || now - storedNewsTimestamp > expirationTime;

            if (storedNews && !isExpired) {
                newsElement.innerHTML = storedNews;
                fadeIn(newsElement);
            } else {
                startLoadingAnimation(newsElement);
                fadeIn(newsElement);
                $.ajax({
                    url: `/api/news/GetSpecificGameNews?appId=${appId}&newsIndex=${newsIndex}`,
                    success: function (data) {
                        if (data.summarizedNews) {
                            localStorage.setItem(storedNewsKey, data.summarizedNews);
                            localStorage.setItem(storedNewsTimestampKey, now);
                            stopLoadingAnimation(newsElement);

                            if (!typingStatus[newsIndex]) {
                                typingStatus[newsIndex] = true;
                                typeWriter(newsElement, data.summarizedNews, 0, () => {
                                    typingStatus[newsIndex] = false;
                                });
                            }
                        } else {
                            console.error("Error: Summarized news not found.");
                            stopLoadingAnimation(newsElement);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.error("Error:", textStatus, errorThrown);
                        stopLoadingAnimation(newsElement);
                    }
                });
            }
        });

        button.innerText = "Show Original";
        button.classList.add("show-original");
    } else {
        fadeOut(newsElement, () => {
            newsElement.innerText = originalContent;
            fadeIn(newsElement);
        });

        button.innerText = "Summarize";
        button.classList.remove("show-original");
    }
}



document.addEventListener("DOMContentLoaded", function () {
    const summarizeButtons = document.querySelectorAll(".summarize-btn");

    summarizeButtons.forEach(button => {
        button.addEventListener("click", function () {
            const appId = button.getAttribute("data-appid");
            const newsIndex = button.getAttribute("data-newsindex");
            summarizeSpecificNews(button, appId, parseInt(newsIndex));
        });

        const appId = button.getAttribute("data-appid");
        const newsIndex = button.getAttribute("data-newsindex");
        const storedNewsKey = `specificNews-${appId}-${newsIndex}`;
        const storedNewsTimestampKey = `specificNewsTimestamp-${appId}-${newsIndex}`;

        const storedNews = localStorage.getItem(storedNewsKey);
        const storedNewsTimestamp = localStorage.getItem(storedNewsTimestampKey);
        const now = new Date().getTime();

        const expirationTime = 60 * 60 * 6000; // 6 hours in milliseconds

        const isExpired = !storedNewsTimestamp || now - storedNewsTimestamp > expirationTime;

        if (storedNews && !isExpired) {
            const newsElement = button.parentElement.nextElementSibling;
            newsElement.innerHTML = storedNews;

            button.innerText = "Show Original";
            button.classList.add("show-original");
        }
    });
});