using Microsoft.AspNetCore.Mvc;
using Recipit.Services.Comments;
using Recipit.ViewModels.Comments;

namespace Recipit.Areas.Follower.Controllers
{
    public class CommentController(ICommentService commentService) : FollowerController
    {
        private readonly ICommentService _commentService = commentService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentSendModel model) => Json(await _commentService.Create(model));
    }
}
