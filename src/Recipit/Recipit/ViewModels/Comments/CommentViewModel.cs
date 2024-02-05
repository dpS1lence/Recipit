using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Mapping;

namespace Recipit.ViewModels.Comments
{
    public class CommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Text { get; set; } = default!;
        public DateTime DatePosted { get; set; }
        public decimal Rating { get; set; }
        public virtual RecipitUser? User { get; set; } = default!;
    }
}
