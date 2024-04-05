$(function () {
    // Attach event handlers using .on()

    // Edit Button
    $(document).on('click', '.edit-btn', function () {
        var container = $(this).closest(".product-item");
        container.find(".view-mode").hide();
        container.find(".edit-mode").show();
    });

    // Cancel Button
    $(document).on('click', '.cancel-btn', function () {
        var container = $(this).closest(".product-item");
        container.find(".edit-mode").hide();
        container.find(".view-mode").show();
    });

    $(document).on('click', '.edit-btn-inrecipe', function () {
        console.log('kurwa');
        var container = $(this).closest(".product-item");
        container.find(".view-mode").hide();
        container.find(".edit-mode").show();
    });

    // Save Button
    $(document).on('click', '.save-btn', function () {
        var container = $(this).closest(".product-item");
        var id = container.data("product-id");
        var name = container.find(".edit-name").val();
        var calories = container.find(".edit-calories").val();
        var image = container.find(".edit-image").val();

        var formData = new FormData();
        formData.append('Id', id);
        formData.append('Name', name);
        formData.append('Calories', calories);
        formData.append('Photo', image);

        console.log(formData);

        $.ajax({
            url: '/administrator/product/edit',
            type: 'PUT',
            processData: false,
            contentType: false,
            data: formData,
            success: function (response) {
                location.reload();
            },
            error: function () {
            }
        });
    });

    // Delete Button
    $(document).on('click', '.delete-btn', function () {
        var container = $(this).closest(".product-item");
        var name = container.find(".edit-name").val();
        if (confirm("Сигурни ли сте че искате да изтриете " + name + " ?")) {
            var id = container.data("product-id");

            $.ajax({
                url: '/administrator/product/delete?id=' + encodeURIComponent(id),
                type: 'DELETE',
                success: function (response) {
                    container.remove();
                },
                error: function () {
                }
            });
        }
    });
});
