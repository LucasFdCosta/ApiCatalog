using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Validation;

namespace Catalog.Api.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The 'name' is required")]
        [StringLength(30, ErrorMessage = "The 'name' cannot have more than 30 characters")]
        [FirstLetterUppercase]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(300)] 
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000, ErrorMessage = "The 'price' has to be between {1} and {2}")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(300)]
        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
    }
}