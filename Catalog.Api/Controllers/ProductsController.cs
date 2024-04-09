using Catalog.Api.Context;
using Catalog.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var products = _context.Products.AsNoTracking().ToList();

        if (products is null) return NotFound("No products found!");

        return Ok(products.ToList());
    }

    [HttpGet("{id:int}", Name="GetProduct")]
    public IActionResult GetById(int id)
    {
        var product = _context.Products.Find(id);

        if (product is null) return NotFound("Product not found!");

        return Ok(product);
    }

    [HttpPost]
    public IActionResult Post(Product product)
    {
        if (product is null) return BadRequest();

        _context.Products.Add(product);
        _context.SaveChanges();

        return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Product product)
    {
        if (id != product.Id) return BadRequest("Invalid ID");

        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);

        if (product is null) return NotFound("Product not found!");

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok($"Deleted product {id}!");
    }
}
