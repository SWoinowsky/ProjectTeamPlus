let isTyping = false;

// fadeIn and fadeOut functions are utility functions for animating opacity transitions.
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

// startLoadingAnimation and stopLoadingAnimation functions are utility functions
// for displaying and hiding loading animations for elements.
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
    // Get the news element and its original content
    const newsElement = button.parentElement.nextElementSibling;
    const originalContent = newsElement.getAttribute("data-original-content");

    // Normalize whitespace for comparison
    let currentNews = normalizeWhitespace(newsElement.innerText);
    let oldNews = normalizeWhitespace(originalContent);

    // Check if the news element currently displays the original content
    if (currentNews === oldNews) {
        // Fade out the current content
        fadeOut(newsElement, () => {
            const now = new Date().getTime();
            const expirationTime = 60 * 60 * 1000; // 1 hour in milliseconds
            const storedNewsKey = `specificNews-${appId}-${newsIndex}`;
            const storedNewsTimestampKey = `specificNewsTimestamp-${appId}-${newsIndex}`;

            // Retrieve the stored news and its timestamp
            const storedNews = localStorage.getItem(storedNewsKey);
            const storedNewsTimestamp = localStorage.getItem(storedNewsTimestampKey);

            // Check if the stored news is expired
            const isExpired = !storedNewsTimestamp || now - storedNewsTimestamp > expirationTime;

            // If the summarized news is stored and not expired, load it
            if (storedNews && !isExpired) {
                newsElement.innerHTML = storedNews;
                fadeIn(newsElement); // Fade in the new content
            } else {
                // If summarized news is not stored or expired, fetch it using AJAX
                startLoadingAnimation(newsElement);
                fadeIn(newsElement);
                $.ajax({
                    url: `/api/news/GetSpecificGameNews?appId=${appId}&newsIndex=${newsIndex}`,
                    success: function (data) {
                        if (data.summarizedNews) {
                            // Store the fetched summarized news and timestamp
                            localStorage.setItem(storedNewsKey, data.summarizedNews);
                            localStorage.setItem(storedNewsTimestampKey, now);
                            stopLoadingAnimation(newsElement);

                            // Use the typeWriter function for animating the display of the summarized news
                            if (!isTyping) {
                                isTyping = true;
                                typeWriter(newsElement, data.summarizedNews, 0, () => {
                                    isTyping = false;
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

        // Toggle the button's inner text and color
        button.innerText = "Show Original";
        button.classList.add("show-original");
    } else {
        // If the news element is not displaying the original content, switch back to it
        fadeOut(newsElement, () => {
            newsElement.innerText = originalContent;
            fadeIn(newsElement);
        });

        // Toggle the button's inner text and color
        button.innerText = "Summarize";
        button.classList.remove("show-original");
    }
}


document.addEventListener("DOMContentLoaded", function () {
    // Get all the summarize buttons
    const summarizeButtons = document.querySelectorAll(".summarize-btn");

    // Add event listener to summarize buttons
    summarizeButtons.forEach(button => {
        button.addEventListener("click", function () {
            const appId = button.getAttribute("data-appid");
            const newsIndex = button.getAttribute("data-newsindex");
            // Call the summarizeSpecificNews function when a button is clicked
            summarizeSpecificNews(button, appId, parseInt(newsIndex));
        });

        // Automatically load summarized news from local storage if available
        const appId = button.getAttribute("data-appid");
        const newsIndex = button.getAttribute("data-newsindex");
        const storedNewsKey = `specificNews-${appId}-${newsIndex}`;
        const storedNewsTimestampKey = `specificNewsTimestamp-${appId}-${newsIndex}`;

        const storedNews = localStorage.getItem(storedNewsKey);
        const storedNewsTimestamp = localStorage.getItem(storedNewsTimestampKey);
        const now = new Date().getTime();

        const expirationTime = 60 * 60 * 1000; // 1 hour in milliseconds

        // Check if the stored news is expired
        const isExpired = !storedNewsTimestamp || now - storedNewsTimestamp > expirationTime;

        // If the summarized news is stored and not expired, load it
        if (storedNews && !isExpired) {
            const newsElement = button.parentElement.nextElementSibling;
            newsElement.innerHTML = storedNews;

            // Toggle the button's inner text and color
            button.innerText = "Show Original";
            button.classList.add("show-original");
        }
    });
});
