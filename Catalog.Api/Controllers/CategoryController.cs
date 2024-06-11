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
    private readonly ICategoryRepository _repository;
    private readonly ILogger _ilogger;

    public CategoryController(ICategoryRepository repository, ILogger<CategoryController> ilogger)
    {
        _repository = repository;
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
        
        var categories = _repository.GetCategories();

        return Ok(categories);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public IActionResult GetById(int id)
    {
        _ilogger.LogInformation($"============= GET api/category/id {id} ===============");
        
        var category = _repository.GetCategory(id);

        return category is not null ? Ok(category) : NotFound($"Category {id} not found...");
    }

    [HttpPost]
    public IActionResult Post(Category category)
    {
        if (category is null) return BadRequest("Invalid data.");

        var created = _repository.Create(category);

        return new CreatedAtRouteResult("GetCategory", new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Category category)
    {
        if (id != category.Id) return BadRequest("Invalid ID");

        _repository.Update(category);

        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var category = _repository.GetCategory(id);

        if (category is null) return NotFound("Category not found!");

        var deleted = _repository.Delete(id);

        return Ok(deleted);
    }
}
