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
    // private readonly IRepository<Product> _repository;
    private readonly IProductRepository _productRepository;

    public ProductsController(/*IRepository<Product> repository, */IProductRepository productRepository)
    {
        // _repository = repository;
        _productRepository = productRepository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var products = _productRepository.GetAll();

        if (products is null) return NotFound("No products found!");

        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public IActionResult GetById(int id)
    {
        var product = _productRepository.Get(p => p.Id == id);

        if (product is null) return NotFound("Product not found!");

        return Ok(product);
    }

    [HttpGet("products/{id}")]
    public IActionResult GetProductsByCategory(int id)
    {
        var products = _productRepository.GetProductsByCategory(id).ToList();

        if (products is null) return NotFound("Product not found!");

        return Ok(products);
    }

    [HttpPost]
    public IActionResult Post(Product product)
    {
        if (product is null) return BadRequest();

        var newProduct = _productRepository.Create(product);

        return new CreatedAtRouteResult("GetProduct", new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Product product)
    {
        if (id != product.Id) return BadRequest("Invalid ID");

        var updatedProduct = _productRepository.Update(product);

        return updatedProduct is not null ? Ok(updatedProduct) : StatusCode(500, $"Error while updating product {id}");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var product = _productRepository.Get(p => p.Id == id);

        if (product is null) return NotFound("Product not found!");

        var deletedProduct = _productRepository.Delete(product);

        return deletedProduct is not null ? Ok($"Successfully deleted product {id}") : StatusCode(500, $"Error while deleting product {id}");
    }
}
