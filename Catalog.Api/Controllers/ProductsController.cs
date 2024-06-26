using Asp.Versioning;
using AutoMapper;
using Catalog.Api.Domain;
using Catalog.Api.DTOs;
using Catalog.Api.Pagination;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace Catalog.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiConventionType(typeof(DefaultApiConventions))]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProductsController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>A list of all products</returns>
    //[Authorize(Policy = "UserOnly")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
    {
        var products = await _uof.ProductRepository.GetAllAsync();

        if (products is null) return NotFound("No products found!");

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParams)
    {
        var products = await _uof.ProductRepository.GetProductsAsync(productsParams);

        return GetProducts(products);
    }

    [HttpGet("filter/price/pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsFilterPrice([FromQuery] ProductsFilterPrice productsFilterPriceParams)
    {
        var products = await _uof.ProductRepository.GetProductsFilterPriceAsync(productsFilterPriceParams);

        return GetProducts(products);
    }

    private ActionResult<IEnumerable<ProductDTO>> GetProducts(IPagedList<Product> products)
    {
        var metadata = new
        {
            products.Count,
            products.PageSize,
            products.PageNumber,
            products.TotalItemCount,
            products.HasNextPage,
            products.HasPreviousPage,
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }

    /// <summary>
    /// Get a product by a given id
    /// </summary>
    /// <param name="id">The product id</param>
    /// <returns>An object with the given id</returns>
    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public async Task<ActionResult<ProductDTO>> GetById(int id)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.Id == id);

        if (product is null) return NotFound("Product not found!");

        var productDto = _mapper.Map<ProductDTO>(product);

        return Ok(productDto);
    }

    [HttpGet("products/{id}")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int id)
    {
        var products = await _uof.ProductRepository.GetProductsByCategoryAsync(id);

        if (products is null) return NotFound("Product not found!");
        
        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDto)
    {
        if (productDto is null) return BadRequest();

        var product = _mapper.Map<Product>(productDto);

        var created = _uof.ProductRepository.Create(product);

        await _uof.CommitAsync();

        var newProductDto = _mapper.Map<ProductDTO>(created);

        return new CreatedAtRouteResult("GetProduct", new { id = newProductDto.Id }, newProductDto);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
    {
        if (patchProductDTO is null || id <= 0) return BadRequest();

        var product = await _uof.ProductRepository.GetAsync(p => p.Id == id);

        if (product is null) return NotFound();

        var productDtoUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

        patchProductDTO.ApplyTo(productDtoUpdateRequest, ModelState);

        if (!ModelState.IsValid || TryValidateModel(productDtoUpdateRequest)) return BadRequest(ModelState);

        _mapper.Map(productDtoUpdateRequest, product);
        _uof.ProductRepository.Update(product);

        await _uof.CommitAsync();

        return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDto)
    {
        if (id != productDto.Id) return BadRequest("Invalid ID");

        var product = _mapper.Map<Product>(productDto);

        var updated = _uof.ProductRepository.Update(product);
        await _uof.CommitAsync();

        var updatedProductDto = _mapper.Map<ProductDTO>(updated);
        
        return Ok(updatedProductDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.Id == id);

        if (product is null) return NotFound("Product not found!");

        var deleted = _uof.ProductRepository.Delete(product);
        await _uof.CommitAsync();

        var deletedProductDto = _mapper.Map<ProductDTO>(deleted);

        return Ok(deletedProductDto);
    }
}
