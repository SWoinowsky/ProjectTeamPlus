$(document).ready(function () {
    // Apply the user's theme preference on page load
    applyThemePreference();

    $("#lightThemeButton, #darkThemeButton").on("click", function () {
        var theme = $(this).data("theme");
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
    });
});

// Function to apply the user's theme preference
function applyThemePreference() {
    var theme = $("#currentTheme").val();
    if (theme) {
        var themeLink = $("#theme-link");
        themeLink.attr("href", "/css/" + theme + "-theme.css");
    }
}
