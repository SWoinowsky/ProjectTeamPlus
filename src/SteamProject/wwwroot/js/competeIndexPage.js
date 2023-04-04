$( function () {
    provideAllCardsWithLink();
    console.log( "Loaded" );
});

function provideAllCardsWithLink() {
    var cards = document.getElementsByClassName("competitionDiv");

    for( card of cards )
    {
        card.onclick = function () {
            location.href = `/compete/Details/${this.id}`;
        }
    }

    cardList = document.getElementsByClassName("competitionDiv");

    console.log( cardList );
}