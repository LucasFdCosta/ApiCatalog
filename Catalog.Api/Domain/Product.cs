﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Catalog.Api.Validation;

namespace Catalog.Api.Domain;

[Table("Products")]
public class Product : IValidatableObject
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The 'name' is required")]
    [StringLength(30, ErrorMessage = "The 'name' cannot have more than 30 characters")]
    [FirstLetterUppercase]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(300)] 
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(1, 10000, ErrorMessage = "The 'price' has to be between {1} and {2}")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(300)]
    public string ImageUrl { get; set; } = string.Empty;
    public float Stock { get; set; }
    public DateTime CreatedAt { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            var firstLetter = Name[0].ToString();

            if (firstLetter != firstLetter.ToUpper())
            {
                yield return new ValidationResult("The first letter of 'name' must be uppercase", new[] { nameof(Name) });
            }
        }

        if (Stock <= 0)
        {
            yield return new ValidationResult("The 'stock' must be bigger than 0", new[] { nameof(Stock) });
        }
    }
}
