using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Domain;

namespace Catalog.Api.DTOs.Mappings
{
    public static class ProductDTOMappingExtensions
    {
        public static ProductDTO? ToProductDTO(this Product product)
        {
            if (product is null) return null;

            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };
        }

        public static Product? ToProduct(this ProductDTO productDto)
        {
            if (productDto is null) return null;

            return new Product()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                ImageUrl = productDto.ImageUrl,
                CategoryId = productDto.CategoryId
            };
        }

        public static IEnumerable<ProductDTO> ToProductDTOList(this IEnumerable<Product> products)
        {
            if (products is null || !products.Any()) return new List<ProductDTO>();

            return products.Select(p => new ProductDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId
            }).ToList();
        }
    }
}