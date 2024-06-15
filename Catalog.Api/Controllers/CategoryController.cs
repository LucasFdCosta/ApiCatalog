using Catalog.Api.Context;
using Catalog.Api.Domain;
using Catalog.Api.DTOs;
using Catalog.Api.DTOs.Mappings;
using Catalog.Api.Filters;
using Catalog.Api.Pagination;
using Catalog.Api.Repositories;
using Catalog.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoryDTO>> Get([FromQuery] CategoriesParameters categoriesParams)
    {
        var categories = _uof.CategoryRepository.GetCategories(categoriesParams);

        return GetCategories(categories);
    }

    [HttpGet("filter/name/pagination")]
    public ActionResult<IEnumerable<CategoryDTO>> GetCategoriesFilterName([FromQuery] CategoriesFilterName categoriesFilterName)
    {
        var filteredCategories = _uof.CategoryRepository.GetCategoriesFilterName(categoriesFilterName);

        return GetCategories(filteredCategories);
    }

    private ActionResult<IEnumerable<CategoryDTO>> GetCategories(PagedList<Category> categories)
    {
        var metadata = new
        {
            categories.TotalCount,
            categories.PageSize,
            categories.CurrentPage,
            categories.TotalPages,
            categories.HasNext,
            categories.HasPrevious,
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<CategoryDTO> GetById(int id)
    {
        _ilogger.LogInformation($"============= GET api/category/id {id} ===============");
        
        var category = _uof.CategoryRepository.Get(c => c.Id == id);

        if (category is null) return NotFound($"Category {id} not found...");

        var categoryDto = category.ToCategoryDTO();

        return Ok(categoryDto);
    }

    [HttpPost]
    public ActionResult<CategoryDTO> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null) return BadRequest("Invalid data.");

        var category = categoryDto.ToCategory();

        var created = _uof.CategoryRepository.Create(category);
        _uof.Commit();

        var newCategoryDto = created.ToCategoryDTO();

        return new CreatedAtRouteResult("GetCategory", new { id = newCategoryDto.Id }, newCategoryDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.Id) return BadRequest("Invalid ID");
        
        var category = categoryDto.ToCategory();

        var updated = _uof.CategoryRepository.Update(category);
        _uof.Commit();

        var updatedCategoryDto = updated.ToCategoryDTO();

        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoryDTO> Delete(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.Id == id);

        if (category is null) return NotFound("Category not found!");

        var deleted = _uof.CategoryRepository.Delete(category);
        _uof.Commit();
        
        var deletedCategoryDto = deleted.ToCategoryDTO();

        return Ok(deletedCategoryDto);
    }
}
