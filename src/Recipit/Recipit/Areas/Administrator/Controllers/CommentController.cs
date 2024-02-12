namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Comments;
    using Recipit.ViewModels.Comments;

    [Route("comments")]
    public class CommentController(ICommentService commentService) : AdministratorController
    {
        private readonly ICommentService _commentService = commentService;
        
        [HttpPost]
        public async Task<IActionResult> Create(CommentSendModel model) => Json(await _commentService.Create(model));

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) => Json(await _commentService.Delete(id));
    }
}
