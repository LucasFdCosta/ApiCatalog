using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
    
        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(30)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}