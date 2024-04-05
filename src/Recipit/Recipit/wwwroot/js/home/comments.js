$(function () {
    const pendingComment = localStorage.getItem('pendingComment');
    if (pendingComment) {
        const commentDetails = JSON.parse(pendingComment);

        if (commentDetails && commentDetails.Text && commentDetails.Text.trim()) {
            $.ajax({
                url: '/follower/comment/create',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    RecipeId: parseInt(commentDetails.RecipeId),
                    Text: commentDetails.Text,
                    Rating: parseInt(commentDetails.Rating)
                }),
                success: function (response) {
                    localStorage.removeItem('pendingComment');
                    location.reload();
                },
                error: function (xhr, status, error) {
                    console.error('Error submitting the pending comment:', error);
                }
            });
        }
    }
});
