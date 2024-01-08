
using Recipit.ViewModels.Comments;

namespace Recipit.Services.Comments
{
    public interface ICommentService
    {
        Task<string> Delete(int id);
        Task<string> Create(CommentViewModel model);
    }
}
