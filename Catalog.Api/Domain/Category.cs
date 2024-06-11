using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Catalog.Api.Domain;

[Table("Categories")]
public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(30)]
    public string ImageUrl { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
