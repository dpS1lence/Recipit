namespace Recipit.Infrastructure.Data.Models
{
    using Recipit.Infrastructure.Data.Models.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a product used in recipes.
    /// </summary>
    public class Product : EntityModel
    {
        public Product() 
        {
            ProductRecipes = new HashSet<ProductRecipe>();
        }

        /// <summary>
        /// Gets or sets the name of the product.
        /// Must be non-empty and cannot exceed 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the calorie count of the product.
        /// Must be a non-negative number.
        /// </summary>
        [Required(ErrorMessage = "Calorie count is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Calorie count must be a non-negative number.")]
        public int Calories { get; set; }

        public virtual ICollection<ProductRecipe> ProductRecipes { get; set; }
    }
}
