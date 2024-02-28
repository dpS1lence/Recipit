namespace Recipit.Services.Products
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Recipit.Contracts;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Pagination;
    using Recipit.Pagination.Contracts;
    using Recipit.ViewModels.Product;
    using System.Linq;

    public class ProductService(RecipitDbContext context, IMapper mapper, ILogger<ProductService> logger) : IProductService
    {
        private readonly RecipitDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger;

        public async Task<IPage<ProductViewModel>> All()
        {
            var products = await _context.Products
                .Include(a => a.ProductRecipes)
                .Where(a => a.ProductRecipes == null || a.ProductRecipes.Count == 0)
                .ToListAsync();

            return new Page<ProductViewModel>(products.Select(_mapper.Map<ProductViewModel>), 1, 10, products.Count);
        }

        public async Task<IEnumerable<ProductViewModel>> SearchProducts(string searchText)
        {
            var products = await _context.Products.AsNoTracking().ToListAsync();
            products = products.Where(a => a.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();

            return products.Select(_mapper.Map<ProductViewModel>);
        }

        public async Task<ProductViewModel> Create(ProductViewModel model)
        {
            Validate.Model(model, _logger);

            var product = _mapper.Map<Product>(model);

            ArgumentNullException.ThrowIfNull(product);
            ArgumentException.ThrowIfNullOrEmpty(product.Name);
            ArgumentException.ThrowIfNullOrEmpty(product.Photo);

            if (product.Calories < 0)
                throw new ArgumentException(nameof(product.Calories));
            else if (await _context.Products.AnyAsync(a => a.Name == product.Name))
                throw new ArgumentException(nameof(product));

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<string> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);

            if (product is not null)
            {
                var isInRecipe = await _context.Recipes.AnyAsync(b => b.Id == product.Id);

                if (!isInRecipe)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return product.Name;
                }
            }

            throw new ArgumentException(nameof(product));
        }

        public async Task<string> Edit(ProductViewModel model)
        {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == model.Id);

            if (product is not null)
            {
                var isInRecipe = await _context.Recipes
                    .Include(a => a.ProductRecipes)
                    .AnyAsync(b => b.ProductRecipes.Any(a => a.ProductId == product.Id));

                if (!isInRecipe)
                {
                    product.Name = model.Name;

                    if (model.Photo is not null)
                    {
                        product.Photo = model.Photo;
                    }

                    product.Calories = model.Calories;

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    return product.Name;
                }
            }

            throw new ArgumentException(nameof(product));
        }
    }
}
