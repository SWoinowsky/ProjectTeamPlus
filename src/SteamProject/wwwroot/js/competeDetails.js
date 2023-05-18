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
        var hasVoted = $(".CurrentVoteId").attr('id') ? true : false;
        var wantsToPlayAgain = $(".CurrentVoteStatus").attr('data-status') === 'true' ? true : false;

        var competitionId = $(".currentCompId").attr('id');

        var voteData = {
            Id: hasVoted ? $(".CurrentVoteId").attr('id') : null,
            CompetitionId: competitionId,
            UserId: $(".currentUserId").attr('id'),
            WantsToPlayAgain: wantsToPlayAgain ? false : true
        };

        var voteUrl = hasVoted ? `/api/Vote/CompetitionVote/${voteData.Id}` : '/api/Vote/CompetitionVote/';

        $.ajax({
            type: hasVoted ? 'PUT' : 'POST',
            url: voteUrl,
            data: JSON.stringify(voteData),
            contentType: 'application/json',
            success: function (data) {
                wantsToPlayAgain = !wantsToPlayAgain;
                $(".CurrentVoteStatus").attr('data-status', wantsToPlayAgain ? 'true' : 'false');
                $("#voteAgainBtn").html(`<i id="voteIcon" class="${wantsToPlayAgain ? 'bi bi-hand-thumbs-down rotate-icon' : 'bi bi-hand-thumbs-up rotate-icon'}"></i> ${wantsToPlayAgain ? 'REVOKE VOTE' : 'VOTE TO COMPETE AGAIN'}`);
                $("#voteAgainBtn").toggleClass('vote-again revoke-vote');
                $("#voteIcon").toggleClass('rotate');
            },
            error: function (data) {
                alert(hasVoted ? 'Error updating vote' : 'Error voting');
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