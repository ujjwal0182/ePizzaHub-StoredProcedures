// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Cart/GetCartItemCount",
        success: function (res) {
            $("#cartCounter").text(res.count); //cartCounter is the id of the element where you want to display the count
        }, error: function (error) {

        }
    })
})