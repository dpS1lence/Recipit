namespace Recipit.Infrastructure.Data.Models
{
    using Recipit.Infrastructure.Data.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a recipe in the application.
    /// </summary>
    public class Recipe : EntityModel
    {
        public Recipe()
        {
            Comments = new HashSet<Comment>();
            ProductRecipes = new HashSet<ProductRecipe>();
        }

        /// <summary>
        /// Gets or sets the name of the recipe.
        /// Must be non-empty and cannot exceed 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Recipe name is required.")]
        [StringLength(100, ErrorMessage = "Recipe name cannot exceed 100 characters.")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description of the recipe.
        /// Must be non-empty and can be up to 1000 characters.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = default!;

        /// <summary>
        /// The user ID of the recipe creator.
        /// </summary>
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the nutritional value of the recipe.
        /// Must be a non-negative number.
        /// </summary>
        [Required(ErrorMessage = "Nutritional value is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Nutritional value must be a non-negative number.")]
        public int NutritionalValue { get; set; }

        /// <summary>
        /// Gets or sets the date when the recipe was published.
        /// Must be a valid date and not in the future.
        /// </summary>
        [Required(ErrorMessage = "Publish date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the photo path of the recipe.
        /// Must be a valid file path format.
        /// </summary>
        [Required(ErrorMessage = "Photo path is required.")]
        public string Photo { get; set; } = default!;

        /// <summary>
        /// Calculates the average rating of the recipe based on comments.
        /// </summary>
        [NotMapped]
        public decimal AverageRating
        {
            get
            {
                if (Comments == null || Comments.Count == 0)
                {
                    return 0;
                }

                return Comments.Average(c => c.Rating);
            }
        }

        /// <summary>
        /// Gets or sets the category of the recipe.
        /// </summary>
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = default!;

        [ForeignKey(nameof(UserId))]
        public virtual RecipitUser User { get; set; } = default!;

        /// <summary>
        /// Navigation property for the products associated with the comments.
        /// </summary>
        public virtual ICollection<Comment> Comments { get; private set; }

        public virtual ICollection<ProductRecipe> ProductRecipes { get; set; }
    }
}
