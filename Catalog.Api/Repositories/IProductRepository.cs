using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Domain;
using Catalog.Api.Pagination;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        // IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
        Task<PagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
        Task<PagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterParameters);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
    }
}