using Catalog.Api.Context;
using Catalog.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var category = _context.Categories.AsNoTracking().ToList();

        if (category is null) return NotFound("No categories found");

        return Ok(category);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public IActionResult GetById(int id)
    {
        var category = _context.Categories.Find(id);

        if (category is null) return NotFound("Category not found");

        return Ok(category);
    }

    [HttpGet]
    [Route("Products")]
    public IActionResult GetCategoriesWithProducts()
    {
        var categories = _context.Categories.Include(c => c.Products).ToList();

        if (categories is null) return NotFound("No categories found");

        return Ok(categories);
    }

    [HttpPost]
    public IActionResult Post(Category category)
    {
        if (category is null) return BadRequest();

        _context.Categories.Add(category);
        _context.SaveChanges();

        return new CreatedAtRouteResult("GetCategory", new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Category category)
    {
        if (id != category.Id) return BadRequest("Invalid ID");

        _context.Entry(category).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var category = _context.Categories.Find(id);

        if (category is null) return NotFound("Category not found!");

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return Ok($"Deleted category {id}!");
    }
}
