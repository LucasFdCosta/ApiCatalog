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
        PagedList<Product> GetProducts(ProductsParameters productsParameters);
        IEnumerable<Product> GetProductsByCategory(int id);
    }
}