using AutoMapper;
using Catalog.Api.Context;
using Catalog.Api.Domain;
using Catalog.Api.DTOs;
using Catalog.Api.DTOs.Mappings;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProductsController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> Get()
    {
        var products = _uof.ProductRepository.GetAll();

        if (products is null) return NotFound("No products found!");

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public ActionResult<ProductDTO> GetById(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.Id == id);

        if (product is null) return NotFound("Product not found!");

        var productDto = _mapper.Map<ProductDTO>(product);

        return Ok(productDto);
    }

    [HttpGet("products/{id}")]
    public ActionResult<IEnumerable<ProductDTO>> GetProductsByCategory(int id)
    {
        var products = _uof.ProductRepository.GetProductsByCategory(id).ToList();

        if (products is null) return NotFound("Product not found!");
        
        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }

    [HttpPost]
    public ActionResult<ProductDTO> Post(ProductDTO productDto)
    {
        if (productDto is null) return BadRequest();

        var product = _mapper.Map<Product>(productDto);

        var created = _uof.ProductRepository.Create(product);

        _uof.Commit();

        var newProductDto = _mapper.Map<ProductDTO>(created);

        return new CreatedAtRouteResult("GetProduct", new { id = newProductDto.Id }, newProductDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProductDTO> Put(int id, ProductDTO productDto)
    {
        if (id != productDto.Id) return BadRequest("Invalid ID");

        var product = _mapper.Map<Product>(productDto);

        var updated = _uof.ProductRepository.Update(product);
        _uof.Commit();

        var updatedProductDto = _mapper.Map<ProductDTO>(updated);
        
        return Ok(updatedProductDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProductDTO> Delete(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.Id == id);

        if (product is null) return NotFound("Product not found!");

        var deleted = _uof.ProductRepository.Delete(product);
        _uof.Commit();

        var deletedProductDto = _mapper.Map<ProductDTO>(deleted);

        return Ok(deletedProductDto);
    }
}
