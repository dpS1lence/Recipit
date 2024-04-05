using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipit.Infrastructure.Data.Models
{
    public class Rating
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the rating of the recipe, from 1 to 5.
        /// </summary>
        [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1 and 5.")]
        public decimal Value { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = default!;

        public RecipitUser User { get; set; } = default!;

        public Recipe Recipe { get; set; } = default!;
    }
}
