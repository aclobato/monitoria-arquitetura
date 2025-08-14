using WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace WebApi.Tests;

public class ProductsControllerTests
{
    [Fact]
    public void Get_ReturnsAllProducts()
    {
        // Arrange
        var controller = new ProductsController();

        // Act
        var result = controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(3, products.Count());
    }

    [Fact]
    public void Get_WithValidId_ReturnsProduct()
    {
        // Arrange
        var controller = new ProductsController();

        // Act
        var result = controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var product = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, product.Id);
        Assert.Equal("Laptop", product.Name);
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var controller = new ProductsController();

        // Act
        var result = controller.Get(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}