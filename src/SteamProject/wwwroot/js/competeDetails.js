$( function () {
    var deleter = document.getElementById("compDeleteBtn");
    var compId = deleter.value;
    deleter.onclick = function () {
        console.log( `DELETING COMPETITION WITH ID ${compId}`)
        Delete( compId );

    }

});

function Delete( int )
{
    $.ajax({
        type: "POST",
        dataType: "json",
        url: `/api/Steam/DeleteComp?compId=${int}`,
        success: successOnAjax,
        error: errorOnAjax
    });
}

function errorOnAjax() {
    console.log( "ERROR in ajax request" );
}

function successOnAjax() {
    console.log( "SUCCESS in ajax request" );
    location.href = `/compete/`;

}