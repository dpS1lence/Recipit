$(document).ready(function () {
    var currentPage = 1;
    var pageSize = 6; // Adjust this as needed
    var totalPages; // Variable to store total pages
    var currentFilterData = {}; // Object to hold the current filter data

    function loadRecipes(page, filterData) {
        $('#recipeSkeleton').show();
        $('#recipeDisplay').hide();

        var url = '/home/recipe/recipes';
        var data = { currentPage: page, pageSize: pageSize };

        if (filterData) { // If there is filter data, change URL and append data
            url = '/home/recipe/filter';
            data = { ...filterData, currentPage: page, pageSize: pageSize };
        }

        $.ajax({
            type: 'GET',
            url: url,
            data: data,
            success: function (response) {
                $('#recipeSkeleton').hide();
                $('#recipeDisplay').show();
                console.log(response);
                if (response.recipes) {
                    displayRecipes(response.recipes); // Pass the 'recipes' object
                } else {
                    console.error('No recipes found in response');
                }
                totalPages = response.totalPages;
                updatePagination(currentPage);
            },
            error: function (error) {
                $('#recipeSkeleton').hide();
                $('#recipeDisplay').show();
                console.log(error);
            }
        });
    }

    // Initial load
    loadRecipes(currentPage);

    $('#prevPage').click(function () {
        console.log("prevPage Current Page before increment:", currentPage);
        if (currentPage > 1) {
            currentPage--;
            loadRecipes(currentPage, currentFilterData);
            console.log("prevPage Current Page after increment:", currentPage);
        }
    });

    $('#nextPage').click(function () {
        console.log(" nextPageCurrent Page before increment:", currentPage);
        if (currentPage < totalPages) {
            currentPage++;
            console.log(" nextPageCurrent Page after increment:", currentPage);
            loadRecipes(currentPage, currentFilterData);
        }
    });

    function updatePagination(page) {
        $('#currentPage').text(page);
        $('#prevPage').prop('disabled', page === 1);
        $('#nextPage').prop('disabled', page === totalPages);
    }

    $('#recipeFilterForm').submit(function (event) {
        event.preventDefault();
        currentPage = 1; // Reset to first page on new filter

        currentFilterData = {
            Name: $('#name').val(),
            Category: $('#category').val(),
            Author: $('#author').val(),
            AverageRating: $('#averageRating').val(),
            NutritionalValue: $('#nutritionalValue').val()
        };

        loadRecipes(currentPage, currentFilterData);
    });

    function displayRecipes(data) {
        if (!data || !data.$values || !Array.isArray(data.$values)) {
            console.error('Invalid data structure:', data);
            return; // Exit the function if data is not as expected
        }

        var recipes = data.$values; // Access the array from $values property
        var html = '';

        recipes.forEach(function (recipe) {

            var stars = generateStarRating(recipe.averageRating);

            var products = recipe.products && recipe.products.$values ? recipe.products.$values : [];

            var ingredientsHtml = '';
            for (var i = 0; i < products.length && i < 5; i++) {
                ingredientsHtml += `<div class="ingridient">
                    <img class="ingridient-image" src="${products[i].photo}" alt="image" />
                    <h1 class="ingridient-title">${products[i].name}</h1>
                </div>`;
            }
            if (products.length > 5) {
                ingredientsHtml += `<div class="ingridient">
                    <h1 class="ingridient-more">+${products.length - 5} Други</h1>
                </div>`;
            }

            var nutritionalValue = recipe.nutritionalValue;
            // Create the element as a string since you're concatenating it into HTML
            var caloriesColorClass = nutritionalValue < 250 ? 'recipe-calories-green' :
                nutritionalValue >= 250 && nutritionalValue <= 450 ? 'recipe-calories-orange' : 'recipe-calories-red';

            // Directly integrate the class determination into your HTML string
            var caloriesHTML = `<h2 class="recipe-calories ${caloriesColorClass}"><i class="fa fa-fire fire-icon"></i>${nutritionalValue} Калории</h2>`;


            // Building the entire recipe HTML
            html += `<div class="recipe-container">
                <div class="recipe-data">
                    <img class="recipe-image" src="${recipe.photo}" alt="image"/>
                    <div class="recipe-title-and-rating">
                        <h1 class="recipe-title">${recipe.name}</h1>
                        <div class="stars-container">${stars}</div>
                    </div>
                </div>
                <div class="recipe-products">
                    <h1 class="recipe-ingridients-title">Продукти</h1>
                    <div class="ingridients-container">${ingredientsHtml}</div>
                </div>
                <div class="recipe-buttons">
                    <button class="view-recipe-btn" data-recipe-id="${recipe.id}"><i class="fa fa-eye"></i>Виж рецептата</button>
                            ${caloriesHTML}
                </div>
            </div>`;
        });
        console.log(html);
        $('#recipeDisplay').html(html);
    }

    // Function to generate star ratings
    function generateStarRating(averageRating) {
        console.log(averageRating);
        var starsHtml = '';
        for (var i = 1; i <= 5; i++) {
            if (i <= averageRating) {
                starsHtml += '<i class="fa-solid fa-star"></i>';
            } else {
                starsHtml += '<i class="fa-regular fa-star"></i>';
            }
        }
        console.log(starsHtml);
        return starsHtml;
    }

    $(document).on('click', '.view-recipe-btn', function () {
        var recipeId = $(this).data('recipe-id');
        window.location.href = `/home/recipe/viewrecipe/${recipeId}`;
    });

});
