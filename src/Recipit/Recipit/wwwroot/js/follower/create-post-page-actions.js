document.addEventListener('DOMContentLoaded', function () {
    var textContent = document.getElementById('text-content');
    var imageContent = document.getElementById('image-content');
    var previewContent = document.getElementById('preview-content');

    var btnPost = document.getElementById('btn-post');
    var btnImage = document.getElementById('btn-image');
    var btnPreview = document.getElementById('btn-preview');
    var btnSubmit = document.getElementById('submit');

    var textDisplayImage = document.getElementById('text-display-image');
    var displayText = document.getElementById('display-text');
    var displayPreview = document.getElementById('display-preview');
    var previewDisplayImage = document.getElementById('preview-display-image');

    function showContent(content) {
        textContent.classList.add('hidden');
        imageContent.classList.add('hidden');
        previewContent.classList.add('hidden');

        content.classList.remove('hidden');
    }

    showContent(textContent);

    btnPost.addEventListener('click', function () {
        showContent(textContent);
        validate();
    });

    btnImage.addEventListener('click', function () {
        showContent(imageContent);
        validate();
    });

    btnPreview.addEventListener('click', function () {
        showContent(previewContent);
        validate();
    });

    textDisplayImage.addEventListener('click', function () {
        showContent(imageContent);
        validate();
    });

    displayText.addEventListener('click', function () {
        showContent(textContent);
        validate();
    });

    displayPreview.addEventListener('click', function () {
        showContent(previewContent);
        validate();
    });

    previewDisplayImage.addEventListener('click', function () {
        showContent(imageContent);
        validate();
    });

    function validate() {
        $('.invalid-input').removeClass('invalid-input');

        var title = document.getElementById('postTitle').value;
        var description = document.getElementById('postDescription').value;
        var cal = document.getElementById('cal').value;
        var category = document.getElementById('cars').value;
        var btn = document.getElementById('submitBtn');

        var isValid = true;
        if (title.trim() === '') {
            $('#postTitle').addClass('invalid-input');
            isValid = false;
        }
        if (description.trim() === '') {
            $('#postDescription').addClass('invalid-input');
            isValid = false;
        }
        if (cal.trim() === '') {
            $('#cal').addClass('invalid-input');
            isValid = false;
        }
        if (category.trim() === '') {
            $('#cars').addClass('invalid-input');
            isValid = false;
        }

        var productsWithQuantities = {};
        $('#product-container .product').each(function () {
            var productName = $(this).find('.product-name').text().trim();
            var quantity = $(this).find('.product-input-for-quantity').val();
            if (!quantity) {
                $(this).find('.product-input-for-quantity').addClass('invalid-input');
                isValid = false;
            }
            productsWithQuantities[productName] = quantity;
        });

        if (!isValid) {
            showContent(textContent);
            return false;
        }
    }
});