$(document).ready(function () {
    // Edit Button
    $(".edit-btn").click(function () {
        var container = $(this).closest(".product-item");
        container.find(".view-mode").hide();
        container.find(".edit-mode").show();
    });

    // Cancel Button
    $(".cancel-btn").click(function () {
        var container = $(this).closest(".product-item");
        container.find(".edit-mode").hide();
        container.find(".view-mode").show();
    });

    // Save Button
    $(".save-btn").click(function () {
        var container = $(this).closest(".product-item");
        var id = container.data("product-id");
        var name = container.find(".edit-name").val();
        var calories = container.find(".edit-calories").val();
        var image = container.find(".edit-image").val();

        // AJAX request to save changes
        $.ajax({
            url: '/administrator/product/edit', // Adjust URL as needed
            type: 'PUT',
            data: JSON.stringify({ id: id, name: name, calories: calories, photo: image }),
            success: function (response) {
                location.reload();
            },
            error: function () {
                alert("An error occurred");
            }
        });
    });

    // Delete Button
    $(".delete-btn").click(function () {
        var container = $(this).closest(".product-item");
        var name = container.find(".edit-name").val();
        if (confirm("Сигурни ли сте че искате да изтриете " + name + " ?")) {
            var container = $(this).closest(".product-item");
            var id = container.data("product-id");

            // AJAX request to delete
            $.ajax({
                url: '/administrator/product/delete',
                type: 'DELETE',
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    container.remove();
                },
                error: function () {
                    alert("An error occurred");
                }
            });
        }
    });
});