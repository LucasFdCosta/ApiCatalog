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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Category> GetCategories(CategoriesParameters categoriesParams)
        {
            var categories = GetAll().OrderBy(c => c.Id).AsQueryable();

            var orderedCategories = PagedList<Category>.ToPagedList(categories, categoriesParams.PageNumber, categoriesParams.PageSize);

            return orderedCategories;
        }

        public PagedList<Category> GetCategoriesFilterName(CategoriesFilterName categoriesFilterName)
        {
            var categories = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriesFilterName.Name))
            {
                categories = categories.Where(c => c.Name.Contains(categoriesFilterName.Name));
            }

            var filteredCategories = PagedList<Category>.ToPagedList(categories, categoriesFilterName.PageNumber, categoriesFilterName.PageSize);

            return filteredCategories;
        }
    }
}