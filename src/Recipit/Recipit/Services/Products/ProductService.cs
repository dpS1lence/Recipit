namespace Recipit.Services.Products
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Pagination.Contracts;
    using Recipit.ViewModels.Product;

    public class ProductService(RecipitDbContext context, UserManager<RecipitUser> userManager, ILogger<ProductService> logger, IMapper mapper) : IProductService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;

        public Task<IPage<ProductViewModel>> All()
        {
            throw new NotImplementedException();
        }

        public Task<ProductViewModel> Create(ProductViewModel model)
        {
            throw new NotImplementedException();
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
