$(document).ready(function () {
    // Star click logic for new comment rating
    $('.add-comment-container .fa-star').click(function () {
        const rating = $(this).data('rating');
        const $ratingContainer = $(this).closest('.comment-rating');

        $ratingContainer.find('.fa-star').removeClass('fa-solid').addClass('fa-regular');
        $ratingContainer.find('.fa-star').each(function (index) {
            if (index < rating) {
                $(this).removeClass('fa-regular').addClass('fa-solid');
            }
        });

        $('#newCommentRating').val(rating);
    });

    // Form submission logic
    $('#commentForm').submit(function (e) {
        e.preventDefault();
        const commentText = $('.comment-textarea').val();
        const rating = $('#newCommentRating').val();

        const urlParts = window.location.pathname.split('/');
        const recipeId = urlParts[urlParts.length - 1];
        console.log(JSON.stringify({
            RecipeId: parseInt(recipeId),
            Text: commentText,
            Rating: parseInt(rating)
        }));
        $.ajax({
            url: '/follower/comment/create',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                RecipeId: parseInt(recipeId),
                Text: commentText,
                Rating: parseInt(rating)
            }),
            success: function (response) {
                location.reload();
            },
            error: function (xhr, status, error) {
                if (xhr.status === 401) {
                    localStorage.setItem('returnUrl', window.location.href);

                    localStorage.setItem('pendingComment', JSON.stringify({
                        RecipeId: recipeId,
                        Text: commentText,
                        Rating: rating
                    }));

                    window.location.href = '/login';
                } else {
                    console.error(error);
                }
            }
        });
    });
});