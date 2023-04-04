$( function () {
    provideAllCardsWithLink();
    console.log( "Loaded" );
});

function provideAllCardsWithLink() {
    var cardList = document.getElementsByClassName("competitionDiv");

    for( card of cardList )
    {
        var id = card.id;

        card.onclick = function() {
            location.href = `/compete/Details/${id}`;
        }
    }
}