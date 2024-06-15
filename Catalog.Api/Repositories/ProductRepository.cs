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

        // public IEnumerable<Product> GetProducts(ProductsParameters productsParams)
        // {
        //     return GetAll()
        //         .OrderBy(p => p.Name)
        //         .Skip((productsParams.PageNumber - 1) * productsParams.PageSize)
        //         .Take(productsParams.PageSize)
        //         .ToList();
        // }

        public PagedList<Product> GetProducts(ProductsParameters productsParams)
        {
            var products = GetAll().OrderBy(p => p.Id).AsQueryable();

            var orderedProducts = PagedList<Product>.ToPagedList(products, productsParams.PageNumber, productsParams.PageSize);

            return orderedProducts;
        }

        public PagedList<Product> GetProductsFilterPrice(ProductsFilterPrice productsFilterPrice)
        {
            var products = GetAll().AsQueryable();

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

            var filteredProducts = PagedList<Product>.ToPagedList(products, productsFilterPrice.PageNumber, productsFilterPrice.PageSize);

            return filteredProducts;
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryid)
        {
            return GetAll().Where(c => c.CategoryId == categoryid);
        }
    }
}