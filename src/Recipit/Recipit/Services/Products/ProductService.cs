namespace Recipit.Services.Products
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Contracts;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Pagination;
    using Recipit.Pagination.Contracts;
    using Recipit.ViewModels.Product;
    using System.Drawing.Printing;
    using System.Linq;

    public class ProductService(RecipitDbContext context, IMapper mapper, ILogger<ProductService> logger) : IProductService
    {
        private readonly RecipitDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger;

        public async Task<IEnumerable<ProductViewModel>> All()
        {
            var productsRaw = await _context.Products.Include(a => a.ProductRecipes).ToListAsync();
            var products = _mapper.Map<IEnumerable<ProductViewModel>>(productsRaw).ToList();

            for (int i = 0; i < productsRaw.Count; i++)
            {
                products[i].IsInRecipe = productsRaw[i].ProductRecipes.Count != 0;
            }

            return products.OrderBy(a => a.IsInRecipe!.Value);
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

            if (product is null)
                throw new ArgumentException(nameof(product));
            else if (string.IsNullOrEmpty(product.Name))
                throw new ArgumentException(nameof(product.Name));
            else if (string.IsNullOrEmpty(product.Photo))
                throw new ArgumentException(nameof(product.Photo));
            else if (product.Calories < 0)
                throw new ArgumentException(nameof(product.Calories));

            if (await _context.Products.AnyAsync(a => a.Name == product.Name))
            {
                throw new ArgumentException("Product already exists!");
            }

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<string> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);

            if (product != null)
            {
                var isInRecipe = await _context.Recipes.AnyAsync(b => b.Id == product.Id);

                if (!isInRecipe)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return product.Name;
                }
            }

            throw new ArgumentException("not deleted");
        }

        public async Task<string> Edit(ProductViewModel model)
        {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == model.Id);

            if (product != null)
            {
                product.Name = model.Name;

                if (model.Photo != null)
                {
                    product.Photo = model.Photo;
                }

                product.Calories = model.Calories;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return product.Name;
            }

            throw new ArgumentException("not updated");
        }

        public async Task<IPage<ProductViewModel>> GetPaginated(int pageIndex, string? name)
        {
            if (pageIndex == 0) pageIndex = 1;
            var pageSize = 40;

            var productsRaw = await _context.Products
                .Include(a => a.ProductRecipes)
                .OrderBy(a => a.ProductRecipes.Count)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if(name is not null)
                productsRaw = await _context.Products
                .Include(a => a.ProductRecipes)
                .OrderBy(a => a.ProductRecipes.Count)
                .Where(a => a.Name.Contains(name))
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var products = _mapper.Map<IEnumerable<ProductViewModel>>(productsRaw)
                .ToList();

            for (int i = 0; i < productsRaw.Count; i++)
            {
                products[i].IsInRecipe = productsRaw[i].ProductRecipes.Count != 0;
            }

            var totalFilteredCount = products.Count;
            var totalPages = (int)Math.Ceiling(totalFilteredCount / (double)pageSize);

            return new Page<ProductViewModel>(products, pageIndex, pageSize, totalPages);
        }
    }
}
