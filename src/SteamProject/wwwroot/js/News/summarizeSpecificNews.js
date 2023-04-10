// Flag to track if the typeWriter function is currently running
let isTyping = false;

// Function to animate typing effect on the provided element
function typeWriter(element, text, i, callback) {

    // Clear the element's inner HTML when starting the typing animation
    if (i === 0) {
        element.innerHTML = "";
    }

    // If there is still text to type, type the next character and set a timeout to continue
    if (i < text.length) {
        element.innerHTML += text.charAt(i);
        setTimeout(() => {
            typeWriter(element, text, i + 1, callback);
        }, 5);
    }
    else
    {
        // Once typing is finished, insert a closing div tag and call the callback if provided
        element.insertAdjacentHTML("afterend", "</div>");
        if (callback) {
            callback();
        }
    }
}

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

// Function to start the loading animation
function startLoadingAnimation(element) {

    // Save the current content in a data attribute
    element.setAttribute("data-original-content", element.innerText);

    // Create a new div with the loading spinner and set its class and style
    const spinnerDiv = document.createElement("div");
    spinnerDiv.classList.add("loading-spinner");
    spinnerDiv.style.display = "flex";
    spinnerDiv.style.justifyContent = "center";
    spinnerDiv.style.alignItems = "center";
    spinnerDiv.style.height = "100%";

    // Create the spinner element and add the Bootstrap spinner class
    const spinner = document.createElement("div");
    spinner.setAttribute("role", "status");

    // Add the spinner element to the spinnerDiv and replace the content of the element with the spinnerDiv
    spinnerDiv.appendChild(spinner);
    element.innerHTML = "";
    element.appendChild(spinnerDiv);
}

// Function to stop the loading animation
function stopLoadingAnimation(element) {

    // Remove the loading spinner div
    const spinnerDiv = element.querySelector(".loading-spinner");
    if (spinnerDiv) {
        element.removeChild(spinnerDiv);
    }
}

function normalizeWhitespace(str) {
    return str.replace(/\s+/g, ' ').trim();
}

function summarizeSpecificNews(button, appId, newsIndex) {
    // Get the news element and original content
    const newsElement = button.parentElement.nextElementSibling;
    const originalContent = newsElement.getAttribute("data-original-content");

    let currentNews = normalizeWhitespace(newsElement.innerText);
    let oldNews = normalizeWhitespace(originalContent);

    // Check if the news element currently displays the original content
    if (currentNews === oldNews) {
        fadeOut(newsElement, () => {
            // If displaying original content, load summarized news
            const now = new Date().getTime();
            const expirationTime = 60 * 60 * 1000; // Set Expiration time here, current is 1 hour in milliseconds
            const storedNewsKey = `specificNews-${appId}-${newsIndex}`;
            const storedNewsTimestampKey = `specificNewsTimestamp-${appId}-${newsIndex}`;

            const storedNews = localStorage.getItem(storedNewsKey);
            const storedNewsTimestamp = localStorage.getItem(storedNewsTimestampKey);

            // Check if the stored news is expired
            const isExpired = !storedNewsTimestamp || now - storedNewsTimestamp > expirationTime;

            // If the summarized news is stored and not expired, load it
            if (storedNews && !isExpired) {
                newsElement.innerHTML = storedNews;
                fadeIn(newsElement);
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

        // Toggle the button's inner text
        button.innerText = "Show Original";
    } else {
        // If the news element is not displaying the original content, switch back to it
        fadeOut(newsElement, () => {
            newsElement.innerText = originalContent;
            fadeIn(newsElement);
        });

        // Toggle the button's inner text
        button.innerText = "Summarize";
    }
}






document.addEventListener("DOMContentLoaded", function () {

    const summarizeButtons = document.querySelectorAll(".summarize-btn");

    // Add event listener to summarize buttons
    summarizeButtons.forEach(button => {
        button.addEventListener("click", function () {
            const appId = button.getAttribute("data-appid");
            const newsIndex = button.getAttribute("data-newsindex");
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

        const isExpired = !storedNewsTimestamp || now - storedNewsTimestamp > expirationTime;

        if (storedNews && !isExpired) {
            const newsElement = button.parentElement.nextElementSibling;
            newsElement.innerHTML = storedNews;

            // Update the button's inner text
            button.innerText = "Show Original";
        }
    });
});