$(function () {
    $('#li-skeleton').hide();
    var searchTimeout;
    var productPopup = $('#product-popup');
    var productList = $('#product-list');
    var productContainer = $('#product-container');
    var submiting = false;

    $(document).on('click', '#btn-add-product', function () {
        productPopup.toggleClass('hidden');
    });

    $('#searchProduct').on('input', function () {
        var searchTerm = $(this).val().trim();

        clearTimeout(searchTimeout);

        searchTimeout = setTimeout(function () {
            if (searchTerm.length > 2) {
                populateProductList(searchTerm);
            } else {
                productList.empty();
            }
        }, 1000);
    });

    $(document).on('click', '#addNewProduct', function () {
        showNewProductForm();
    });


    if (!submiting) {
        $(document).on('click', '#submitNewProduct', function () {
            submiting = true;
            console.log('submiting now : ' + submiting);
            var newProductName = $('#newProductName').val();
            var newProductPhoto = $('#newProductPhoto').val();
            var newProductCalories = $('#newProductCalories').val();

            $.ajax({
                url: '/follower/product/create',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    Name: newProductName,
                    Photo: newProductPhoto,
                    Calories: newProductCalories
                }),
                success: function (data) {
                    submiting = false;
                    fillFormForSearch();
                },
                error: function (error) {
                    submiting = false;
                    fillFormForExists();
                }
            });
        });
    }

    function populateProductList(searchTerm) {
        $('#li-skeleton').show();
        productList.empty();
        $.ajax({
            url: '/home/product/search',
            type: 'GET',
            contentType: 'application/json',
            data: { searchTerm: searchTerm },
            success: function (data) {
                $('#li-skeleton').hide();
                console.log(data);
                var products = data.$values;
                for (var i = 0; i < products.length; i++) {
                    var currentProduct = products[i];
                    var listItem = $('<li>').text(currentProduct.name);
                    productList.append(listItem);
                    attachClickEvent(listItem, currentProduct.name);
                }
                function attachClickEvent(item, productName) {
                    item.click(function () {
                        var selectedProduct = productName;
                        addProductToContainer(selectedProduct);
                        productPopup.addClass('hidden');
                    });
                }
            },
            error: function (error) {
                $('#li-skeleton').hide();
                console.log(error);
            }
        });
    }

    function addProductToContainer(productName) {
        if (!isProductAlreadyAdded(productName)) {
            var productItem = $('<div class="product">');
            productItem.append('<div class="x-icon" onclick="removeProduct(this)"><i class="fa fa-trash-can"></i></div>');
            productItem.append('<h1 class="product-name">' + productName + '</h1>');
            productItem.append('<input required class="product-input-for-quantity" type="text" placeholder="Количество" />');
        }

        productContainer.append(productItem);
    }
    function isProductAlreadyAdded(productName) {
        var existingProducts = productContainer.find('.product-name').map(function () {
            return $(this).text().trim().toLowerCase();
        }).get();

        return existingProducts.includes(productName.trim().toLowerCase());
    }

    window.removeProduct = function (element) {
        $(element).closest('.product').remove();
    };

    function showNewProductForm() {
        fillFormForCreate();
    }

    function fillFormForSearch() {
        productPopup.empty();
        productPopup.append('<input type="text" class="search-products" id="searchProduct" placeholder="Търси продукт">');
        productPopup.append('<ul id="product-list" class="product-list">');
        productPopup.append('</ul>');
        productPopup.append('<button class="add-new-product-btn" id="addNewProduct">Добави нов продукт</button>');
        $('#searchProduct').on('input', function () {
            var searchTerm = $(this).val().trim();

            clearTimeout(searchTimeout);

            searchTimeout = setTimeout(function () {
                if (searchTerm.length > 2) {
                    populateProductList(searchTerm);
                } else {
                    productList.empty();
                }
            }, 200);
        });

        $(document).on('click', '#addNewProduct', function () {
            showNewProductForm();
            console.log('addNewProduct');
        });
        productList = $('#product-list');
    }
    function fillFormForCreate() {
        productPopup.empty();
        productPopup.append('<input class="search-products" type="text" id="newProductName" placeholder="Име на продукта">');
        productPopup.append('<input class="search-products" type="text" id="newProductPhoto" placeholder="Снимка (линк)">');
        productPopup.append('<input class="search-products" type="number" id="newProductCalories" placeholder="Калории">');
        productPopup.append('<button class="add-new-product-btn" id="submitNewProduct">Създай</button>');
        productPopup.append('<button class="add-new-product-btn" id="cancelNewProduct">Откажи</button>');
        $(document).on('click', '#cancelNewProduct', function () {
            fillFormForSearch();
        });
    }
    function fillFormForExists() {
        productPopup.empty();
        productPopup.append('<h1 class="exists-product" id="exists-product">Продуктът вече съществува!</h1>');
        searchTimeout = setTimeout(function () {
            showNewProductForm();
        }, 3000);
    }
});