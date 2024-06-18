using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Domain;
using Catalog.Api.Pagination;
using X.PagedList;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        // IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
        Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
        Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterParameters);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
    }
}