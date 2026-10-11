function AddItemToCart(ItemId, Quantity) {
    $.ajax({
        type: "POST",
        url: "/Cart/AddToCart/" + ItemId + "/" + Quantity,
        success: function (res) { $("#cartCounter").text(res.count); },
        error: function (xhr) { alert(xhr.responseJSON?.message ?? "Could not add item."); }
    });
}

function updateQuantity(id, currentQuantity, quantity) {
    if (parseInt(quantity) < 0) return;
    $.ajax({
        type: "PUT",
        url: "/Cart/UpdateQuantity/" + id + "/" + quantity,
        success: function (res) {
            $("#cartCounter").text(res.count);
            location.reload();                  // totals dobara dikhane ke liye
        },
        error: function (xhr) { alert(xhr.responseJSON?.message ?? "Could not update item."); }
    });
}

function deleteItem(id) {
    $.ajax({
        type: "DELETE",
        url: "/Cart/DeleteItem/" + id,
        success: function (res) {
            $("#cartCounter").text(res.count);
            location.reload();
        },
        error: function (xhr) { alert(xhr.responseJSON?.message ?? "Could not remove item."); }
    });
}