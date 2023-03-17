$( function () {
    modalActivations();
});

function modalActivations() {
    var closeBtn = document.getElementById("modalCloser");
    document.getElementById("ModalBtn").onclick = function() { $("#compareModal").modal("toggle") };

    closeBtn.onclick = function() {
        $("#compareModal").modal("hide");
    }
}