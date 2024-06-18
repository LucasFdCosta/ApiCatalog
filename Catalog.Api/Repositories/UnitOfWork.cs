using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Context;

namespace Catalog.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository? _productRepo;
        private ICategoryRepository? _categoryRepo;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository
        { 
            get
            {
                return _productRepo = _productRepo ?? new ProductRepository(_context);
            }
        }

        public ICategoryRepository CategoryRepository
        { 
            get
            {
                return _categoryRepo = _categoryRepo ?? new CategoryRepository(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}