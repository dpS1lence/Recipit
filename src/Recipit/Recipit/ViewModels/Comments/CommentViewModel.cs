using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Mapping;
using Recipit.ViewModels.Account;

namespace Recipit.ViewModels.Comments
{
    public class CommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Text { get; set; } = default!;
        public DateTime DatePosted { get; set; }
        public decimal Rating { get; set; }
        public virtual UserViewModel? User { get; set; } = default!;
    }
}
