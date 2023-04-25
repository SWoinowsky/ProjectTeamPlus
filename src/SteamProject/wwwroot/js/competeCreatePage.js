$( function () {
    showCurrentIndex();
    checkSelect();
});

function findForm() {
    return document.getElementById("formInternals")
};

function showCurrentIndex() {
    categories = ["Duel", "Free-For-All"];

    var selector = document.getElementById("categorySelect");
    var index = selector.selectedIndex;
    var text = selector.children[index].value;
    
    console.log(text);
    if( text == categories[0] )
    {
        showDuel();
    }
    else if( text == categories[1] )
    {
        showFFA();
    }
}

function checkSelect() {
    var selector = document.getElementById("categorySelect");

    selector.onchange = function() {
        showCurrentIndex();
    }
}

function showFFA() {
    var pageForm = findForm();
    pageForm.innerHTML = "";

    var eleNew = document.createElement('div');
    eleNew.className = "DynamicInput";
    eleNew.innerHTML = "THIS IS THE FREE FOR ALL RESULT";

    pageForm.append(eleNew);
}

function showDuel() {
    var pageForm = findForm();
    pageForm.innerHTML = "";


    var eleNew = document.createElement('div');
    eleNew.className = "DynamicInput";
    eleNew.innerHTML = "THIS IS THE DUEL RESULT";

    pageForm.append(eleNew);
}