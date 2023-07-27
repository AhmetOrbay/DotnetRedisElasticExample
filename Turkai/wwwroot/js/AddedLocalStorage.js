function AddedLocalStorage(data, Id) {
    var LocalData = JSON.parse(localStorage.getItem("Basket"));
    if (LocalData) {
        var existingItemIndex = LocalData.findIndex(function (item) {
            return item.Id === Id;
        });

        if (existingItemIndex !== -1) {

            LocalData[existingItemIndex].Count += data.Count;
        } else {

            LocalData.push(data);
        }
    } else {

        LocalData = [data];
    }
    localStorage.setItem("Basket", JSON.stringify(LocalData));

}