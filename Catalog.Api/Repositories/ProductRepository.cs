using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Context;
using Catalog.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public Product GetProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            return product;
        }

        public IQueryable<Product> GetProducts()
        {
            return _context.Products;
        }

        public Product Create(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(Product));

            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        public bool Update(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(Product));

            if (_context.Products.Any(p => p.Id == product.Id))
            {
                _context.Products.Update(product);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product is not null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}