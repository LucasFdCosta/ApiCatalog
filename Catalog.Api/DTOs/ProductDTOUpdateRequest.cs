using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.DTOs
{
    public class ProductDTOUpdateRequest : IValidatableObject
    {
        [Range(1, 9999, ErrorMessage = "Stock must be between {1} and {2}")]
        public float Stock { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreatedAt.Date <= DateTime.Today.Date)
            {
                yield return new ValidationResult("The 'CreatedAt' value cannot be before today", new[] {nameof(CreatedAt)});
            };
        }
    }
}