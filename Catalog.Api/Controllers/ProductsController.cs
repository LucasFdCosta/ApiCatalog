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

    public ProductsController(IUnitOfWork uof)
    {
        _uof = uof;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> Get()
    {
        var products = _uof.ProductRepository.GetAll();

        if (products is null) return NotFound("No products found!");

        var productsDto = products.ToProductDTOList();

        return Ok(productsDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public ActionResult<ProductDTO> GetById(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.Id == id);

        if (product is null) return NotFound("Product not found!");

        var productDto = product.ToProductDTO();

        return Ok(productDto);
    }

    [HttpGet("products/{id}")]
    public ActionResult<IEnumerable<ProductDTO>> GetProductsByCategory(int id)
    {
        var products = _uof.ProductRepository.GetProductsByCategory(id).ToList();

        if (products is null) return NotFound("Product not found!");
        
        var productsDto = products.ToProductDTOList();

        return Ok(productsDto);
    }

    [HttpPost]
    public ActionResult<ProductDTO> Post(ProductDTO productDto)
    {
        if (productDto is null) return BadRequest();

        var product = productDto.ToProduct();

        var created = _uof.ProductRepository.Create(product);
        _uof.Commit();

        var newProductDto = created.ToProductDTO();

        return new CreatedAtRouteResult("GetProduct", new { id = newProductDto.Id }, newProductDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProductDTO> Put(int id, ProductDTO productDto)
    {
        if (id != productDto.Id) return BadRequest("Invalid ID");

        var product = productDto.ToProduct();

        var updated = _uof.ProductRepository.Update(product);
        _uof.Commit();

        var updatedProductDto = updated.ToProductDTO();
        
        return Ok(updatedProductDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProductDTO> Delete(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.Id == id);

        if (product is null) return NotFound("Product not found!");

        var deleted = _uof.ProductRepository.Delete(product);
        _uof.Commit();

        var deletedProductDto = deleted.ToProductDTO();

        return Ok(deletedProductDto);
    }
}
