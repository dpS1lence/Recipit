namespace Recipit.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Infrastructure.Data.Models;

    public class RecipitDbContext(DbContextOptions<RecipitDbContext> options)
        : IdentityDbContext<RecipitUser>(options)
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductRecipe> ProductsRecipies { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductRecipe>()
                .HasKey(pr => new { pr.ProductId, pr.RecipeId });

            modelBuilder.Entity<Comment>()
                .Property(c => c.Rating)
                .HasPrecision(3, 2);
        }
    }
}
