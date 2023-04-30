$(document).ready(function () {
    // Apply the user's theme preference on page load
    applyThemePreference();

    $("#color_mode").on("change", function () {
        var theme = $(this).prop("checked") ? "dark" : "light";
        updateTheme(theme);
    });
});

// Function to apply the user's theme preference
function applyThemePreference() {
    var theme = $("#currentTheme").val();
    if (theme) {
        var themeLink = $("#theme-link");
        themeLink.attr("href", "/css/" + theme + "-theme.css");

        // Set the initial state of the toggle switch
        if (theme === "dark") {
            $("#color_mode").prop("checked", true);
        } else {
            $("#color_mode").prop("checked", false);
        }
    }
}

// Function to update the theme
function updateTheme(theme) {
    var url = `/api/Steam/UpdateTheme?theme=${theme}`;

    console.log("Updating theme to:", theme); // Log the theme
    console.log("API URL:", url); // Log the API URL

    $.ajax({
        type: "POST",
        dataType: "text",
        url: url,
        success: function () {
            location.reload();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error updating theme: ", jqXHR);
        },
    });
}