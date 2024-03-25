namespace Recipit.Infrastructure.Data.Models
{
    using Recipit.Infrastructure.Data.Models.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a comment made by a user on a recipe.
    /// </summary>
    public class Comment : EntityModel
    {
        /// <summary>
        /// Gets or sets the ID of the user who made the comment.
        /// </summary>
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the ID of the recipe the comment is associated with.
        /// </summary>
        [Required(ErrorMessage = "Recipe ID is required.")]
        public int RecipeId { get; set; }

        /// <summary>
        /// Gets or sets the text content of the comment.
        /// Must be non-empty and can be up to 500 characters.
        /// </summary>
        [Required(ErrorMessage = "Comment text is required.")]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string Text { get; set; } = default!;

        /// <summary>
        /// Gets or sets the date and time when the comment was posted.
        /// </summary>
        [Required(ErrorMessage = "Date of comment is required.")]
        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual RecipitUser User { get; set; } = default!;
    }
}
