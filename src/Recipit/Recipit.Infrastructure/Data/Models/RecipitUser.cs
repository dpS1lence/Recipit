namespace Recipit.Infrastructure.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class RecipitUser : IdentityUser
    {
        public RecipitUser()
        {
            Recipes = [];
            Comments = [];
            Ratings = [];
        }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// Must be non-empty and cannot exceed 50 characters.
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// Must be non-empty and cannot exceed 50 characters.
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the creation date of the user.
        /// </summary>
        [Required(ErrorMessage = "CreationDate is required.")]
        public DateTime CreationDate { get; set; } = default!;

        /// <summary>
        /// Gets or sets the photo path of the user.
        /// Must be a valid file path format.
        /// </summary>
        [Required(ErrorMessage = "Photo path is required.")]
        public string Photo { get; set; } = default!;

        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
