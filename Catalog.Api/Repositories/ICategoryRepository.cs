using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Domain;

namespace Catalog.Api.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}