function dynamidLoad(page) {
    var isInRecipeContainer;
    var isInRecipeDiv;
    var isInRecipeButtons;
    var isLoading = false;
    $(window).scroll(function () {
        if (!isLoading && $(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            isLoading = true;
            $.ajax({
                url: '/allpaginated',
                data: { pageIndex: page + 1, json: true },
                type: 'GET',
                success: function (result) {
                    page++;
                    result.$values.forEach(function (product) {
                        if (product.isInRecipe) {
                            isInRecipeContainer = 'in-recipe';
                            isInRecipeDiv = 'enabled';
                            isInRecipeButtons = 'dissabled';
                        } else {
                            isInRecipeContainer = '';
                            isInRecipeDiv = 'dissabled';
                            isInRecipeButtons = '';
                        }

                        var productHtml = `
                                    <div class="recipe-container product-item ${isInRecipeContainer}" data-product-id="${product.id}">
                    <div class="recipe-data">
                        <!-- View Mode -->
                        <div class="view-mode">
                            <img src="${product.photo}" alt="${product.name}" class="recipe-image" />
                            <div class="recipe-title-and-rating">
                                <h3 class="recipe-title">${product.name}</h3>
                                <p class="recipe-calories">${product.calories} кал</p>
                            </div>
                            <div class="${isInRecipeDiv}">
                                <h1><i class="fa-solid fa-lock"></i> В Рецепта</h1>
                                <a class="edit-btn-inrecipe">Редактирай въпреки това</a>
                            </div>
                            <div class="recipe-buttons ${isInRecipeButtons}">
                                <button class="edit-btn">Редактирай</button>
                                <button class="delete-btn">Изтрий</button>
                            </div>
                        </div>
                        <!-- Edit Mode -->
                        <div class="edit-mode" style="display:none;">
                            <input required type="text" class="edit-name" value="${product.name}" />
                            <input required type="text" class="edit-calories" value="${product.calories}" />
                            <input required type="text" class="edit-image" value="${product.photo}" />
                            <div class="recipe-buttons">
                                <button class="save-btn">Запази</button>
                                <button class="cancel-btn">Откажи</button>
                            </div>
                        </div>
                    </div>
                </div>`;
                        console.log(productHtml);
                        $('.recipes-container').append(productHtml);
                    });


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
                },
                complete: function () {
                    isLoading = false;
                }
            });
        }
    });
}