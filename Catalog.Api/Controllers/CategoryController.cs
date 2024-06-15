using Catalog.Api.Context;
using Catalog.Api.Domain;
using Catalog.Api.DTOs;
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
    public ActionResult<IEnumerable<CategoryDTO>> Get()
    {
        _ilogger.LogInformation($"============= GET api/category ===============");
        
        var categories = _uof.CategoryRepository.GetAll();

        var categoriesDto = new List<CategoryDTO>();

        foreach (var category in categories)
        {
            categoriesDto.Add(new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl
            });
        }

        return Ok(categoriesDto);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<CategoryDTO> GetById(int id)
    {
        _ilogger.LogInformation($"============= GET api/category/id {id} ===============");
        
        var category = _uof.CategoryRepository.Get(c => c.Id == id);

        if (category is null) return NotFound($"Category {id} not found...");

        var categoryDto = new CategoryDTO()
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl
        };

        return Ok(categoryDto);
    }

    [HttpPost]
    public ActionResult<CategoryDTO> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null) return BadRequest("Invalid data.");

        var category = new Category()
        {
            // Id = categoryDto.Id,
            Name = categoryDto.Name,
            ImageUrl = categoryDto.ImageUrl
        };

        var created = _uof.CategoryRepository.Create(category);
        _uof.Commit();

        var newCategoryDto = new CategoryDTO()
        {
            Id = created.Id,
            Name = created.Name,
            ImageUrl = created.ImageUrl
        };

        return new CreatedAtRouteResult("GetCategory", new { id = newCategoryDto.Id }, newCategoryDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.Id) return BadRequest("Invalid ID");
        
        var category = new Category()
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            ImageUrl = categoryDto.ImageUrl
        };

        var updated = _uof.CategoryRepository.Update(category);
        _uof.Commit();

        var updatedCategoryDto = new CategoryDTO()
        {
            Id = updated.Id,
            Name = updated.Name,
            ImageUrl = updated.ImageUrl
        };

        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoryDTO> Delete(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.Id == id);

        if (category is null) return NotFound("Category not found!");

        var deleted = _uof.CategoryRepository.Delete(category);
        _uof.Commit();
        
        var deletedCategoryDto = new CategoryDTO()
        {
            Id = deleted.Id,
            Name = deleted.Name,
            ImageUrl = deleted.ImageUrl
        };

        return Ok(deletedCategoryDto);
    }
}
