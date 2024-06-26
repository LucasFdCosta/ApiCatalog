﻿using Catalog.Api.Controllers;
using Catalog.Api.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UnitTests.UnitTests
{
    public class GetProductUnitTest : IClassFixture<ProductsUnitTestController>
    {
        private readonly ProductsController _controller;

        public GetProductUnitTest(ProductsUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task GetProductById_OKResult()
        {
            // Arrange
            var id = 17;

            // Act
            var data = await _controller.GetById(id);

            // Assert
            // using xUnit
            var okResult = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal(200, okResult.StatusCode);
            // /\ this does the same as this \/
            // using FluentAssertions
            data.Result.Should().BeOfType<OkObjectResult>() // verify if the type of the result is OkObjectResult
                .Which.StatusCode.Should().Be(200); // verify if the status code is 200
        }

        [Fact]
        public async Task GetProductById_NotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var data = await _controller.GetById(id);

            // Assert
            data.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetProducts_Return_ProductDTOList()
        {
            // Arrange
            

            // Act
            var data = await _controller.Get();

            // Assert
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<ProductDTO>>()
                .And.NotBeNull();
        }
    }
}
