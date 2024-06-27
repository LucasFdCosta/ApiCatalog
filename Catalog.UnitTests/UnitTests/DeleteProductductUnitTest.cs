using Catalog.Api.Controllers;
using Catalog.Api.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UnitTests.UnitTests
{
    public class DeleteProductUnitTest : IClassFixture<ProductsUnitTestController>
    {
        private readonly ProductsController _controller;

        public DeleteProductUnitTest(ProductsUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task DeleteProduct_Return_OkResult()
        {
            // Arrange
            var id = 17;

            // Act
            var data = await _controller.Delete(id);

            // Assert
            var createdResult = data.Result.Should().BeOfType<OkObjectResult>();
            createdResult.Subject.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeleteProduct_Return_NotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var data = await _controller.Delete(id);

            // Assert
            var createdResult = data.Result.Should().BeOfType<NotFoundObjectResult>();
            createdResult.Subject.StatusCode.Should().Be(404);
        }
    }
}
