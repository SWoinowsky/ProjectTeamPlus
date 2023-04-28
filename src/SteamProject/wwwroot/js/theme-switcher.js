$(document).ready(function () {
    $("#lightThemeButton, #darkThemeButton").on("click", function () {
        var theme = $(this).data("theme");
        var url = "/api/steam/updatetheme";

        $.ajax({
            type: "POST",
            dataType: "text",
            url: `/api/Steam/UpdateTheme?theme=${theme}`,
            success: function () {
                location.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error updating theme: ", jqXHR);
            },
        });
    });
});

