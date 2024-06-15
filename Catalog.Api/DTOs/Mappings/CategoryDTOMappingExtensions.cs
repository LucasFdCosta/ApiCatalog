using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Domain;

namespace Catalog.Api.DTOs.Mappings
{
    public static class CategoryDTOMappingExtensions
    {
        public static CategoryDTO? ToCategoryDTO(this Category category)
        {
            if (category is null) return null;

            return new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl
            };
        }

        public static Category? ToCategoryDTO(this CategoryDTO categoryDto)
        {
            if (categoryDto is null) return null;

            return new Category()
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                ImageUrl = categoryDto.ImageUrl
            };
        }

        public static IEnumerable<CategoryDTO> ToCategoryDTOList(this IEnumerable<Category> categories)
        {
            if (categories is null || !categories.Any()) return new List<CategoryDTO>();

            return categories.Select(c => new CategoryDTO()
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl
            }).ToList();
        }
    }
}