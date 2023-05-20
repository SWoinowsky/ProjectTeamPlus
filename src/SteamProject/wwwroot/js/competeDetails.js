$(function () {
    var deleter = document.getElementById("compDeleteBtn");
    var compId = deleter.value;
    deleter.onclick = function () {
        console.log(`DELETING COMPETITION WITH ID ${compId}`)
        Delete(compId);
    }

    // Vote to compete again
    var voter = document.getElementById("voteAgainBtn");
    if (voter) { // make sure the button exists
        voter.onclick = function () {
            var competitionId = this.value;
            var userId = // get the user id, this depends on how you store it

                $.ajax({
                    type: 'POST',
                    url: '/api/VoteController/competitionvote',
                    data: JSON.stringify({ CompetitionId: competitionId, UserId: userId, WantsToPlayAgain: true }),
                    contentType: 'application/json',
                    success: function (data) {
                        alert('Vote submitted successfully');
                        // handle successful voting, for example, hide the button
                        $('#voteAgainBtn').hide();
                    },
                    error: function (data) {
                        alert('Error voting');
                        // handle error case
                    }
                });
        }
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