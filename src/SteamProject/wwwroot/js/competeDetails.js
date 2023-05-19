$(function () {
    var deleter = document.getElementById("compDeleteBtn");
    var compId = deleter.value;
    deleter.onclick = function () {
        console.log(`DELETING COMPETITION WITH ID ${compId}`)
        Delete(compId);
    }
});

$(document).ready(function () {
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