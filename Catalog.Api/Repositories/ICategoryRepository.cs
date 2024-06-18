using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Domain;
using Catalog.Api.Pagination;

namespace Catalog.Api.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters);
        Task<PagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesFilterName);
    }
}