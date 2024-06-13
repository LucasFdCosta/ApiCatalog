using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Domain;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Product GetProduct(int id);
        Product Create(Product Product);
        bool Update(Product Product);
        bool Delete(int id);
    }
}