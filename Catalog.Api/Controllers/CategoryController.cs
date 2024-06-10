using Catalog.Api.Context;
using Catalog.Api.Domain;
using Catalog.Api.Filters;
using Catalog.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger _ilogger;

    public CategoryController(AppDbContext context, ILogger<CategoryController> ilogger)
    {
        _context = context;
        _ilogger = ilogger;
    }

    [HttpGet("UsingFromServices/{name}")]
    public IActionResult GetHelloFromServices([FromServices] IMyService service, string name)
    {
        return Ok(service.Hello(name));
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public IActionResult Get()
    {
        _ilogger.LogInformation($"============= GET api/category ===============");
        
        var category = _context.Categories.AsNoTracking().ToList();

        if (category is null) return NotFound("No categories found");

        return Ok(category);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public IActionResult GetById(int id)
    {
        _ilogger.LogInformation($"============= GET api/category/id {id} ===============");
        
        var category = _context.Categories.Find(id);

        if (category is null)
        {
            _ilogger.LogInformation($"============= GET api/category/id {id} NOT FOUND =========");
            return NotFound("Category not found");
        };

        return Ok(category);
    }

    [HttpGet]
    [Route("Products")]
    public IActionResult GetCategoriesWithProducts()
    {
        _ilogger.LogInformation($"============= GET api/category/products ===============");

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
