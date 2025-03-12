using NUnit.Framework;
using BlazorServerCRUD.Web.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServerCRUD.Tests;

[TestFixture]
public class WeatherForecastServiceTests
{
    private WeatherForecastService _service;
    private DateTime _testDate;

    [SetUp]
    public void Setup()
    {
        _service = new WeatherForecastService();
        _testDate = new DateTime(2025, 3, 12);
    }

    [Test]
    public async Task GetForecastForCityAsync_Seattle_ReturnsValidForecast()
    {
        // Act
        var forecast = await _service.GetForecastForCityAsync("Seattle", _testDate);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(forecast, Is.Not.Null);
            Assert.That(forecast.Date, Is.EqualTo(_testDate));
            Assert.That(forecast.TemperatureC, Is.InRange(-5, 25));
            Assert.That(forecast.Summary, Is.AnyOf("Chilly", "Cool", "Mild", "Rainy"));
        });
    }

    [Test]
    public async Task GetForecastForCityAsync_Phoenix_ReturnsValidForecast()
    {
        // Act
        var forecast = await _service.GetForecastForCityAsync("Phoenix", _testDate);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(forecast, Is.Not.Null);
            Assert.That(forecast.TemperatureC, Is.InRange(20, 45));
            Assert.That(forecast.Summary, Is.AnyOf("Hot", "Sweltering", "Scorching", "Dry"));
        });
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void GetForecastForCityAsync_InvalidCity_ThrowsArgumentException(string city)
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(
            async () => await _service.GetForecastForCityAsync(city, _testDate));
        
        Assert.That(ex.Message, Does.Contain("City name cannot be empty"));
        Assert.That(ex.ParamName, Is.EqualTo("city"));
    }

    [Test]
    public void GetForecastForCityAsync_UnknownCity_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _service.GetForecastForCityAsync("NonExistentCity", _testDate));
        
        Assert.That(ex.Message, Does.Contain("Weather data not available for city"));
    }
}
