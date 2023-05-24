$(function () {
    var deleter = document.getElementById("compDeleteBtn");
    var compId = deleter.value;
    deleter.onclick = function () {
        console.log(`DELETING COMPETITION WITH ID ${compId}`);
        Delete(compId);
    }
});

$(document).ready(function () {
    $("#selectGameButton").click(function () {
        $("#gameSelectModal").modal("show");
    });
});



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

                //alert('Game vote updated successfully, Please reload the page.');
            },
            error: function (data) {
                alert('Error updating vote');
            }
        });
    });

    function updateVoteCount() {
        var goalExists = document.getElementById("GoalPresent");
        if(goalExists == null)
        {
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
    }

    $('#gameSelectModal').on('show.bs.modal', function (e) {
        updateSharedGamesList();
    })

    // Fetch shared games for the competition
    function updateSharedGamesList(updatedVoteStatus) {
        var competitionId = $(".currentCompId").attr('id');
        var userId = $(".currentUserId").attr('id');

        $.ajax({
            type: 'GET',
            url: `/api/Vote/SharedGames/${competitionId}`,
            success: function (games) {
                $('#sharedGamesList').empty();

                if (games.length === 0) {
                    $('#sharedGamesList').append('<p class="no-games-found light">No shared games found between SIN users. Ensure the user is on the platform and has shared games with you.</p>');
                } else {
                    games.forEach(function (game) {
                        var gameVoteStatus = game.currentUserVote;

                        $('#sharedGamesList').append(`
                    <div class="col-sm-4">
                        <div class="card sharedCard" id="${game.id}" data-vote-status="${gameVoteStatus}">
                            <img src="https://steamcdn-a.akamaihd.net/steam/apps/${game.appId}/header.jpg" class="card-img-top" alt="${game.name}">
                            <div class="card-body">
                                <h5 class="card-title">${game.name}</h5>
                                <p class="card-text vote-count" id="vote-${game.id}">Votes: </p>
                            </div>
                        </div>
                    </div>`);

                        attachGameClickHandler(game.id, userId, competitionId, gameVoteStatus);

                        // Now fetch the votes for this game
                        getGameVotes(game.id, competitionId);
                    });
                }
            },
            error: function () {
                alert('Error getting shared games');
            }
        });
    }


    function getGameVotes(gameId, competitionId) {
        $.ajax({
            type: 'GET',
            url: `/api/Vote/GameVotes/${gameId}/${competitionId}`,
            success: function (game) {
                var gameVoteStatus = game.currentUserVote ? 'true' : 'false';
                // Update the vote count for the game
                $(`#vote-${gameId}`).text(`Votes: ${game.voteCount}`);
                // Update the data-vote-status attribute
                $(`#${gameId}`).data('vote-status', gameVoteStatus);
            },
            error: function () {
                alert(`Error getting votes for game ${gameId}`);
            }
        });
    }


    function attachGameClickHandler(gameId, userId, competitionId, currentVoteStatus) {
        $(`#${gameId}`).click(function () {
            console.log(`currentVoteStatus: ${currentVoteStatus}`);

            var voteData = {
                GameId: gameId,
                UserId: userId,
                WantsToPlay: !currentVoteStatus,
                CompetitionId: competitionId,
            };


            $.ajax({
                type: 'PUT',
                url: '/api/Vote/GameVote/',
                data: JSON.stringify(voteData),
                contentType: 'application/json',
                success: function (data) {
                    var updatedVoteStatus = data.vote ? 'true' : 'false';
                    $(`#${gameId}`).data('vote-status', updatedVoteStatus);
                    updateSharedGamesList(updatedVoteStatus); // update the shared games list after voting

                    getGameVotes(gameId, competitionId); // update the vote count after voting

                    //alert('Game vote updated successfully, Please reload the page.');
                },
                error: function (data) {
                    alert('Error updating game vote');
                }
            });
        });
    }
});

function showRunSubmissionModal(compId)
{
    var modal = document.getElementById("run-submission");
    $("#run-submission").modal("show");
}

function closeModal()
{
    window.location.reload()
}

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
