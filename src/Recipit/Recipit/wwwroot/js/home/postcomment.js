$(document).ready(function () {
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
                Text: commentText
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