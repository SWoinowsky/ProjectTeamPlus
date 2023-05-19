$(function () {
    var deleter = document.getElementById("compDeleteBtn");
    var compId = deleter.value;
    deleter.onclick = function () {
        console.log(`DELETING COMPETITION WITH ID ${compId}`);
        Delete(compId);
    }
});

function updateVoteCount() {
    var competitionId = $(".currentCompId").attr('id');
    $.ajax({
        type: 'GET',
        url: `/api/Vote/CompetitionVoteCount/${competitionId}`,
        success: function (voteData) {
            $.ajax({
                type: 'GET',
                url: `/api/Vote/TotalCompetitionUsers/${competitionId}`,
                success: function (userData) {
                    $("#voteCount").text(`Votes: ${voteData}/${userData}`);
                },
                error: function (userData) {
                    alert('Error getting total users count');
                }
            });
        },
        error: function (voteData) {
            alert('Error getting vote count');
        }
    });
}

function updateSharedGamesList() {
    var competitionId = $(".currentCompId").attr('id');
    $.ajax({
        type: 'GET',
        url: `/api/Vote/SharedGames/${competitionId}`,
        success: function (sharedGames) {
            // Clear the sharedGamesList div
            $('#sharedGamesList').empty();

            // Append a new card for each shared game
            sharedGames.forEach(function (game) {
                var gameCard = `
                    <div class="col-sm-4">
                        <div class="card">
                            <img src="${game.IconUrl}" class="card-img-top" alt="${game.Name}">
                            <div class="card-body">
                                <h5 class="card-title">${game.Name}</h5>
                                <p class="card-text">Click the image to vote for this game.</p>
                            </div>
                        </div>
                    </div>`;

                $('#sharedGamesList').append(gameCard);

                // Add click event for each game card image to vote for this game
                $(`.card-img-top[src="${game.IconUrl}"]`).click(function () {
                    var voteData = {
                        GameId: game.Id,
                        UserId: $(".currentUserId").attr('id'),
                        WantsToPlay: true // set this to true as user wants to play this game when they click on it
                    };

                    $.ajax({
                        type: 'PUT',
                        url: '/api/Vote/GameVote/',
                        data: JSON.stringify(voteData),
                        contentType: 'application/json',
                        success: function (data) {
                            alert('Game vote updated successfully');
                        },
                        error: function (data) {
                            alert('Error updating game vote');
                        }
                    });
                });
            });
        },

        error: function () {
            alert('Error getting shared games');
        }
    });
}


$(document).ready(function () {
    var compStatus = $('body').data('comp-status');

    if (compStatus === 'GameSelection') {
        updateSharedGamesList();
        $('#gameSelectModal').modal('show');
    }
    // Fetch the initial vote count when the page loads
    updateVoteCount();

    $("#voteAgainBtn").click(function () {
        var wantsToPlayAgain = $(".CurrentVoteStatus").attr('data-status') === 'true' ? true : false;
        var competitionId = $(".currentCompId").attr('id');
        var voteData = {
            CompetitionId: competitionId,
            UserId: $(".currentUserId").attr('id'),
            WantsToPlayAgain: !wantsToPlayAgain // invert the current status
        };

        $.ajax({
            type: 'PUT',
            url: '/api/Vote/CompetitionVote/',
            data: JSON.stringify(voteData),
            contentType: 'application/json',
            success: function (data) {
                // Extract the updated value from the response
                wantsToPlayAgain = data.wantsToPlayAgain;
                $(".CurrentVoteStatus").attr('data-status', wantsToPlayAgain ? 'true' : 'false');
                $("#voteAgainBtn").html(`<i id="voteIcon" class="${wantsToPlayAgain ? 'bi bi-hand-thumbs-down' : 'bi bi-hand-thumbs-up'}"></i> ${wantsToPlayAgain ? 'REVOKE VOTE' : 'VOTE TO COMPETE AGAIN'}`);
                $("#voteAgainBtn").toggleClass('vote-again revoke-vote');

                // Update the vote count
                updateVoteCount();
            },
            error: function (data) {
                alert('Error updating vote');
            }
        });
    });


});


function Delete(int) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: `/api/Steam/DeleteComp?compId=${int}`,
        success: successOnAjax,
        error: errorOnAjax
    });
}

function errorOnAjax() {
    console.log("ERROR in ajax request");
}

function successOnAjax() {
    console.log("SUCCESS in ajax request");
    location.href = `/compete/`;
}
