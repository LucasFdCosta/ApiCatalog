using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Pagination
{
    public class ProductsFilterPrice : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public string PriceCriterion { get; set; } = string.Empty; // gt, lt, eq
    }
}