﻿namespace Recipit.Services.Products
{
    using Recipit.Pagination.Contracts;
    using Recipit.ViewModels.Product;

    public interface IProductService
    {
        Task<IPage<ProductViewModel>> GetPaginated(int pageIndex, string? name);
        Task<IEnumerable<ProductViewModel>> All();
        Task<ProductViewModel> Create(ProductViewModel model);
        Task<string> Delete(int id);
        Task<string> Edit(ProductViewModel model);
        Task<IEnumerable<ProductViewModel>> SearchProducts(string searchText);
    }
}
