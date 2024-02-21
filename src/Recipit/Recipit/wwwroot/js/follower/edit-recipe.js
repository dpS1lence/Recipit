$(function () {
    $(document).on('click', '#submitBtn', function () {
        var title = document.getElementById('postTitle').value;
        var description = document.getElementById('postDescription').value;
        var cal = document.getElementById('cal').value;
        var category = document.getElementById('cars').value;
        var btn = document.getElementById('submitBtn');
        var productsWithQuantities = {};
        $('#product-container .product').each(function () {
            var productName = $(this).find('.product-name').text().trim();
            var quantity = $(this).find('.product-input-for-quantity').val();
            productsWithQuantities[productName] = quantity;
        });

        var imageData = document.getElementById('image').files[0];

        var formData = new FormData();
        formData.append('Name', title);
        formData.append('Description', description);
        formData.append('Calories', cal);
        formData.append('Category', category);
        formData.append('Products', JSON.stringify(productsWithQuantities));
        formData.append('Photo', imageData);
        
        btn.style.cursor = 'wait';
        btn.style.pointerEvents = 'none';
        $.ajax({
            url: '/follower/recipe/edit',
            type: 'POST',
            processData: false,
            contentType: false,
            data: formData,
            enctype: 'multipart/form-data',
            success: function (data) {
                document.getElementById('postTitle').value = '';
                document.getElementById('postDescription').value = '';
                document.getElementById('cars').value = '';
                
                $('#image').val('');

                $('#product-container').empty();
                location.reload();
            },
            error: function (error) {
                location.reload();
            }
        });
    });
});