using Catalog.Api.Context;
using Catalog.Api.Domain;
using Catalog.Api.Filters;
using Catalog.Api.Repositories;
using Catalog.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger _ilogger;

    public CategoryController(IUnitOfWork uof,ILogger<CategoryController> ilogger)
    {
        _uof = uof;
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
        
        var categories = _uof.CategoryRepository.GetAll();

        return Ok(categories);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public IActionResult GetById(int id)
    {
        _ilogger.LogInformation($"============= GET api/category/id {id} ===============");
        
        var category = _uof.CategoryRepository.Get(c => c.Id == id);

        return category is not null ? Ok(category) : NotFound($"Category {id} not found...");
    }

    [HttpPost]
    public IActionResult Post(Category category)
    {
        if (category is null) return BadRequest("Invalid data.");

        var created = _uof.CategoryRepository.Create(category);
        _uof.Commit();

        return new CreatedAtRouteResult("GetCategory", new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Category category)
    {
        if (id != category.Id) return BadRequest("Invalid ID");

        _uof.CategoryRepository.Update(category);
        _uof.Commit();

        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.Id == id);

        if (category is null) return NotFound("Category not found!");

        var deleted = _uof.CategoryRepository.Delete(category);
        _uof.Commit();

        return Ok(deleted);
    }
}
