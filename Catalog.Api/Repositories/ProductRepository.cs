using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Context;
using Catalog.Api.Domain;
using Catalog.Api.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetProducts(ProductsParameters productsParams)
        {
            return GetAll()
                .OrderBy(p => p.Name)
                .Skip((productsParams.PageNumber - 1) * productsParams.PageSize)
                .Take(productsParams.PageSize)
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryid)
        {
            return GetAll().Where(c => c.CategoryId == categoryid);
        }
    }
}