namespace Recipit.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents the association between a Product and a Recipe.
    /// This is a join class in a many-to-many relationship.
    /// </summary>
    public class ProductRecipe
    {
        /// <summary>
        /// Gets or sets the foreign key for the associated Product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity details for the associated Product.
        /// </summary>
        public string QuantityDetails { get; set; } = default!;

        /// <summary>
        /// Navigation property for the associated Product.
        /// </summary>
        public virtual Product Product { get; set; } = default!;

        /// <summary>
        /// Gets or sets the foreign key for the associated Recipe.
        /// </summary>
        public int RecipeId { get; set; }

        /// <summary>
        /// Navigation property for the associated Recipe.
        /// </summary>
        public virtual Recipe Recipe { get; set; } = default!;
    }
}
