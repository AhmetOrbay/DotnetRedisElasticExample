function AddedSepet(Id, Title) {
    var count = document.getElementById("basketSize-" + Id).value
    $("#errorText").text("");
    $("#errorPopup").hide();
    $.ajax({
        url: "/Home/BasketCheck",
        type: "POST",
        data: {
            basketDetail: [{
                Id: Id,
                Title: Title,
                Count: count
            }]
        },
        success: function (result) {
            if (result.checkStock == false) {
                $("#errorText").text(result.title + " stock quantity is not enough");
                $("#errorPopup").show();
            } else {
                AddedLocalStorage({
                    Id: Id,
                    Title: Title,
                    Count: parseInt(count)
            }
        },
        error: function (xhr, status, error) {

            console.error("Hata: " + error);
        }
    });
}
