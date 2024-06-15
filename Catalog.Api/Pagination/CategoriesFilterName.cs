using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Pagination
{
    public class CategoriesFilterName : QueryStringParameters
    {
        public string Name { get; set; } = string.Empty;
    }
}