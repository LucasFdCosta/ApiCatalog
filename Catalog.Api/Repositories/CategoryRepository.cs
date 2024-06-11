using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Context;
using Catalog.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.Include(c => c.Products).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
        }

        public Category Create(Category category)
        {
            if (category is null) throw new ArgumentNullException(nameof(Category));

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category;
        }

        public Category Update(Category category)
        {
            if (category is null) throw new ArgumentNullException(nameof(Category));

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return category;
        }

        public Category Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category is null) throw new ArgumentNullException(nameof(Category));

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return category;
        }
    }
}