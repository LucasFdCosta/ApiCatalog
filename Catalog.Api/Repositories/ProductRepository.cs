using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Context;
using Catalog.Api.Domain;
using Catalog.Api.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        // public IEnumerable<Product> GetProducts(ProductsParameters productsParams)
        // {
        //     return GetAll()
        //         .OrderBy(p => p.Name)
        //         .Skip((productsParams.PageNumber - 1) * productsParams.PageSize)
        //         .Take(productsParams.PageSize)
        //         .ToList();
        // }

        public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams)
        {
            var products = await GetAllAsync();

            var orderedProducts = products.OrderBy(p => p.Id).AsQueryable();
            
            // var result = PagedList<Product>.ToPagedList(orderedProducts, productsParams.PageNumber, productsParams.PageSize);
            var result = await orderedProducts.ToPagedListAsync(productsParams.PageNumber, productsParams.PageSize);

            return result;
        }

        public async Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterPrice)
        {
            var products = await GetAllAsync();

            if (productsFilterPrice.Price.HasValue && !string.IsNullOrEmpty(productsFilterPrice.PriceCriterion))
            {
                if (productsFilterPrice.PriceCriterion.Equals("gt", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price > productsFilterPrice.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsFilterPrice.PriceCriterion.Equals("lt", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price < productsFilterPrice.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsFilterPrice.PriceCriterion.Equals("eq", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price == productsFilterPrice.Price.Value).OrderBy(p => p.Price);
                }
                else
                {
                    throw new ArgumentException($"'{productsFilterPrice.PriceCriterion}' is not a valid {nameof(productsFilterPrice.PriceCriterion)} value");
                }
            }

            // var filteredProducts = PagedList<Product>.ToPagedList(products.AsQueryable(), productsFilterPrice.PageNumber, productsFilterPrice.PageSize);
            var filteredProducts = products.ToPagedListAsync(productsFilterPrice.PageNumber, productsFilterPrice.PageSize);

            return await filteredProducts;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryid)
        {
            var products = await GetAllAsync();

            var productsByCategory = products.Where(c => c.CategoryId == categoryid);

            return productsByCategory;
        }
    }
}