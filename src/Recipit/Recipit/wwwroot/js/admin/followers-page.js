$(function () {
    $(document).on('click', '.delete-btn', function () {
        if (confirm("Сигурни ли сте че искате да изтриете последователя?")) {
            var followerId = $(this).data("follower-id");

            $.ajax({
                url: '/followers/delete/' + encodeURIComponent(followerId),
                type: 'GET',
                success: function (response) {
                    location.reload();
                },
                error: function (xhr, status, error) {
                    location.reload();
                }
            });
        }
    });
});
