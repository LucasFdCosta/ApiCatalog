using Asp.Versioning;
using Catalog.Api.Domain;
using Catalog.Api.DTOs;
using Catalog.Api.DTOs.Mappings;
using Catalog.Api.Filters;
using Catalog.Api.Pagination;
using Catalog.Api.Repositories;
using Catalog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace Catalog.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[EnableCors(PolicyName = "allowedOrigins")]
[Produces("application/json")]
//[EnableRateLimiting(policyName: "fixedwindow")]
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

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>A list of all categories</returns>
    [HttpGet]
    [Authorize]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    //[DisableRateLimiting]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        _ilogger.LogInformation($"============= GET api/category ===============");
        
        var categories = await _uof.CategoryRepository.GetAllAsync();

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    /// <summary>
    /// Get all categories with pagination
    /// </summary>
    /// <param name="categoriesParams">The pagination parameters</param>
    /// <returns>All categories separated by pages</returns>
    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParams)
    {
        var categories = await _uof.CategoryRepository.GetCategoriesAsync(categoriesParams);

        return GetCategories(categories);
    }

    /// <summary>
    /// Get all categories by name with pagination
    /// </summary>
    /// <param name="categoriesFilterName">The name and the pagination parameters</param>
    /// <returns>All categories with the given name, separated by pages</returns>
    [HttpGet("filter/name/pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesFilterName([FromQuery] CategoriesFilterName categoriesFilterName)
    {
        var filteredCategories = await _uof.CategoryRepository.GetCategoriesFilterNameAsync(categoriesFilterName);

        return GetCategories(filteredCategories);
    }

    private ActionResult<IEnumerable<CategoryDTO>> GetCategories(IPagedList<Category> categories)
    {
        var metadata = new
        {
            categories.Count,
            categories.PageSize,
            categories.PageCount,
            categories.TotalItemCount,
            categories.HasNextPage,
            categories.HasPreviousPage,
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    /// <summary>
    /// Get a category by id
    /// </summary>
    /// <param name="id">The category id</param>
    /// <returns>A single category with the given id</returns>
    [DisableCors]
    [HttpGet("{id:int}", Name = "GetCategory")]
    [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDTO>> GetById(int id)
    {
        _ilogger.LogInformation($"============= GET api/category/id {id} ===============");
        
        var category = await _uof.CategoryRepository.GetAsync(c => c.Id == id);

        if (category is null) return NotFound($"Category {id} not found...");

        var categoryDto = category.ToCategoryDTO();

        return Ok(categoryDto);
    }

    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <remarks>
    ///     Request example:
    ///     POST api/v1/categories
    ///     {
    ///         "name": "category9",
    ///         "imageUrl": "category9.jpg"
    ///     }
    /// </remarks>
    /// <param name="categoryDto">An category object</param>
    /// <returns>The created category</returns>
    /// <remarks>Returns the created category</remarks>
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null) return BadRequest("Invalid data.");

        var category = categoryDto.ToCategory();

        var created = _uof.CategoryRepository.Create(category);
        await _uof.CommitAsync();

        var newCategoryDto = created.ToCategoryDTO();

        return new CreatedAtRouteResult("GetCategory", new { id = newCategoryDto.Id }, newCategoryDto);
    }

    [HttpPut("{id:int}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.Id) return BadRequest("Invalid ID");
        
        var category = categoryDto.ToCategory();

        var updated = _uof.CategoryRepository.Update(category);
        await _uof.CommitAsync();

        var updatedCategoryDto = updated.ToCategoryDTO();

        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<CategoryDTO>> Delete(int id)
    {
        var category = await _uof.CategoryRepository.GetAsync(c => c.Id == id);

        if (category is null) return NotFound("Category not found!");

        var deleted = _uof.CategoryRepository.Delete(category);
        await _uof.CommitAsync();
        
        var deletedCategoryDto = deleted.ToCategoryDTO();

        return Ok(deletedCategoryDto);
    }
}
