using Catalog.Api.Controllers;
using Catalog.Api.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UnitTests.UnitTests
{
    public class PutProductUnitTest : IClassFixture<ProductsUnitTestController>
    {
        private readonly ProductsController _controller;

        public PutProductUnitTest(ProductsUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduct_Return_OkResult()
        {
            // Arrange
            var id = 16;

            var newProductDto = new ProductDTO
            {
                Id = id,
                Name = "New Product updated",
                Price = 10.99M,
                ImageUrl = "image123.jpg",
                CategoryId = 1
            };

            // Act
            var data = await _controller.Put(id, newProductDto) as ActionResult<ProductDTO>;

            // Assert
            var createdResult = data.Result.Should().BeOfType<OkObjectResult>();
            createdResult.Subject.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task PutProduct_Return_BadRequest()
        {
            // Arrange
            var id = 999;

            var newProductDto = new ProductDTO
            {
                Id = 998,
                Name = "New Product updated",
                Price = 10.99M,
                ImageUrl = "image123.jpg",
                CategoryId = 1
            };

            // Act
            var data = await _controller.Put(id, newProductDto);

            // Assert
            var createdResult = data.Result.Should().BeOfType<BadRequestObjectResult>();
            createdResult.Subject.StatusCode.Should().Be(400);
        }
    }
}
