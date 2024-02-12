namespace Recipit.Services.Products
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Pagination;
    using Recipit.Pagination.Contracts;
    using Recipit.ViewModels.Product;
    using System.Linq;

    public class ProductService(RecipitDbContext context, IMapper mapper) : IProductService
    {
        private readonly RecipitDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<IPage<ProductViewModel>> All()
        {
            var products = await _context.Products.ToListAsync();

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
            var product = _mapper.Map<Product>(model);

            if(await _context.Products.AnyAsync(a => a.Name == product.Name))
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

            if(product != null)
            {
                var isInRecipe = await _context.Recipes.AnyAsync(b => b.Id == product.Id);

                if(!isInRecipe)
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
                var isInRecipe = await _context.Recipes.AnyAsync(b => b.Id == product.Id);

                if (!isInRecipe)
                {
                    product.Name = model.Name;
                    product.Photo = model.Photo;
                    product.Calories = model.Calories;

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    return product.Name;
                }
            }

            throw new ArgumentException("not updated");
        }

        public Task<ProductViewModel> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
