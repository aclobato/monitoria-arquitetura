using WebApi.Controllers;
using Xunit;

namespace WebApi.Tests;

public class WeatherForecastControllerTests
{
    [Fact]
    public void Get_ReturnsWeatherForecasts()
    {
        // Arrange
        var controller = new WeatherForecastController();

        // Act
        var result = controller.Get();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public void Get_ReturnsValidTemperatureRange()
    {
        // Arrange
        var controller = new WeatherForecastController();

        // Act
        var result = controller.Get();

        // Assert
        Assert.All(result, forecast => 
        {
            Assert.InRange(forecast.TemperatureC, -20, 55);
        });
    }
}