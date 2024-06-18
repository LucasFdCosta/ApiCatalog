using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Domain;
using Catalog.Api.Pagination;
using X.PagedList;

namespace Catalog.Api.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters);
        Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesFilterName);
    }
}