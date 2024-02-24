
using Recipit.ViewModels.Comments;

namespace Recipit.Services.Comments
{
    public interface ICommentService
    {
        Task Delete(int id);
        Task<int> Create(CommentSendModel model);
    }
}
