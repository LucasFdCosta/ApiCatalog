using System.Collections.ObjectModel;

namespace Catalog.Api.Domain;

public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<Product>? Products { get; set; }
}
