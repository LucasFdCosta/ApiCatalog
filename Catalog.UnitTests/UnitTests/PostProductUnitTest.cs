using Catalog.Api.Controllers;
using Catalog.Api.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UnitTests.UnitTests
{
    public class PostProductUnitTest : IClassFixture<ProductsUnitTestController>
    {
        private readonly ProductsController _controller;

        public PostProductUnitTest(ProductsUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PostProduct_Return_CreatedStatusCode()
        {
            // Arrange
            var newProductDto = new ProductDTO
            {
                Name = "New Product",
                Price = 10.99M,
                ImageUrl = "image123.jpg",
                CategoryId = 1
            };

            // Act
            var data = await _controller.Post(newProductDto);

            // Assert
            var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
            createdResult.Subject.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task PostProduct_Return_BadRequest()
        {
            // Arrange
            ProductDTO newProductDto = null;

            // Act
            var data = await _controller.Post(newProductDto);

            // Assert
            var badRequestResult = data.Result.Should().BeOfType<BadRequestResult>();
            badRequestResult.Subject.StatusCode.Should().Be(400);
        }
    }
}
