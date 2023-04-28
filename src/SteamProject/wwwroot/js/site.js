// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Global Javascript Code
function showAwardPopup(badgeArray) {
    // Fetch badge details (name, image, etc.) from the server and populate the badge-info div.
    var baseUrl = window.location.origin;

    badgeArray.forEach(function (badgeId) {
        $.ajax({
            url: baseUrl + "/api/Badge/GetBadgeDetails/" + badgeId,
            type: "GET",
            success: function (badge) {
                // Populate the badge-info div with the fetched badge details.
                var badgeInfo = document.getElementById("badge-info");
                badgeInfo.innerHTML += `
                <h3>${badge.name}</h3>
                <img class="badgeImg" src="/api/Badge/GetBadgeImage/${badge.id}" alt="${badge.name}" />
                <p>${badge.description}</p>
            `;

                // Display the popup
                document.getElementById("badge-award-popup").style.display = "block";
            }
        });
    });
}


function closeAwardPopup() {
    document.getElementById("badge-award-popup").style.display = "none";
}

document.getElementById("close-badge-popup").addEventListener("click", closeAwardPopup);

function checkForNewBadges() {
    // Get the base URL for your application
    var baseUrl = window.location.origin;

    $.ajax({
        url: baseUrl + "/api/Badge/CheckForNewBadges",
        type: "GET",
        success: BadgeChecker = function (badgeId) {
            success: function (badgeId) {
            if (badgeId) {
                // Display the badge award popup
                showAwardPopup(badgeId);
            }
        }
    });
}

// Call checkForNewBadges() periodically, e.g., every minute:
setInterval(checkForNewBadges, 6000);
