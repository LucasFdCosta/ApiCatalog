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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams)
        {
            var categories = await GetAllAsync();

            var orderedCategories = categories.OrderBy(c => c.Id).AsQueryable();

            // var result = PagedList<Category>.ToPagedList(orderedCategories, categoriesParams.PageNumber, categoriesParams.PageSize);
            var result = await orderedCategories.ToPagedListAsync(categoriesParams.PageNumber, categoriesParams.PageSize);
            
            return result;
        }

        public async Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesFilterName)
        {
            var categories = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriesFilterName.Name))
            {
                categories = categories.Where(c => c.Name.Contains(categoriesFilterName.Name));
            }

            // var filteredCategories = PagedList<Category>.ToPagedList(categories.AsQueryable(), categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
            var filteredCategories = await categories.ToPagedListAsync(categoriesFilterName.PageNumber, categoriesFilterName.PageSize);

            return filteredCategories;
        }
    }
}