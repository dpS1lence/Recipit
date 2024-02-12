$(function () {
    $(document).on('click', '.delete-btn', function () {
        if (confirm("Сигурни ли сте че искате да изтриете последователя?")) {
            var followerId = $(this).data("follower-id");
            var container = $(this).closest(".product-item");

            $.ajax({
                url: '/followers/delete?followerId=' + encodeURIComponent(followerId),
                type: 'DELETE',
                success: function (response) {
                    container.fadeOut("slow", function () { $(this).remove(); });
                    alert("Follower removed successfully.");
                },
                error: function (xhr, status, error) {
                    alert("Error removing follower: " + error);
                }
            });
        }
    });
});
