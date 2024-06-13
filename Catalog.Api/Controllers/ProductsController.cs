using Catalog.Api.Context;
using Catalog.Api.Domain;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var products = _repository.GetProducts().ToList();

        if (products is null) return NotFound("No products found!");

        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public IActionResult GetById(int id)
    {
        var product = _repository.GetProduct(id);

        if (product is null) return NotFound("Product not found!");

        return Ok(product);
    }

    [HttpPost]
    public IActionResult Post(Product product)
    {
        if (product is null) return BadRequest();

        var newProduct = _repository.Create(product);

        return new CreatedAtRouteResult("GetProduct", new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Product product)
    {
        if (id != product.Id) return BadRequest("Invalid ID");

        bool updated = _repository.Update(product);

        return updated ? Ok(product) : StatusCode(500, $"Error while updating product {id}");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        bool deleted = _repository.Delete(id);

        return deleted ? Ok($"Successfully deleted product {id}") : StatusCode(500, $"Error while deleting product {id}");
    }
}
