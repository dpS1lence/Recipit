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

        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Edit(ProductViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ProductViewModel> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
