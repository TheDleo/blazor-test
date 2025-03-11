using Bunit;
using NUnit.Framework;
using Moq;
using BlazorServerCRUD.Web.Data;
using BlazorServerCRUD.Web.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace BlazorServerCRUD.Tests.Pages;

[TestFixture]
public class FetchDataTests
{
    private Mock<IWeatherForecastService> _weatherForecastServiceMock;
    private WeatherForecast[] _testForecasts;
    private Bunit.TestContext _testContext;

    [SetUp]
    public void Setup()
    {
        // Create test data first
        _testForecasts = new[]
        {
            new WeatherForecast
            {
                Date = DateTime.Now.Date,
                TemperatureC = 20,
                Summary = "Warm"
            },
            new WeatherForecast
            {
                Date = DateTime.Now.Date.AddDays(1),
                TemperatureC = 25,
                Summary = "Hot"
            }
        };

        // Create a new test context for each test
        _testContext = new Bunit.TestContext();

        // Setup the mock
        _weatherForecastServiceMock = new Mock<IWeatherForecastService>();
        _testContext.Services.AddSingleton<IWeatherForecastService>(_weatherForecastServiceMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

    [Test]
    public void FetchData_WhenInitiallyLoaded_ShowsLoadingMessage()
    {
        // Arrange
        _weatherForecastServiceMock
            .Setup(service => service.GetForecastAsync(It.IsAny<DateTime>()))
            .Returns(new TaskCompletionSource<WeatherForecast[]>().Task); // Never completes

        // Act
        var cut = _testContext.RenderComponent<FetchData>();

        // Assert
        var loadingMessage = cut.Find("p em");
        Assert.That(loadingMessage.TextContent, Is.EqualTo("Loading..."));
    }

    [Test]
    public async Task FetchData_WhenDataLoaded_DisplaysWeatherData()
    {
        // Arrange
        _weatherForecastServiceMock
            .Setup(service => service.GetForecastAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(_testForecasts);

        // Act
        var cut = _testContext.RenderComponent<FetchData>();
        
        // Wait for the component to render and stabilize
        await Task.Delay(50); // Small delay to allow async operation to complete
        cut.Render(); // Force a re-render

        // Assert
        var rows = cut.FindAll("tbody tr");
        Assert.That(rows, Has.Count.EqualTo(2));

        // Verify first row data
        var firstRow = rows[0];
        Assert.Multiple(() =>
        {
            Assert.That(firstRow.QuerySelector("td:nth-child(1)")?.TextContent, 
                Is.EqualTo(_testForecasts[0].Date.ToShortDateString()));
            Assert.That(firstRow.QuerySelector("td:nth-child(2)")?.TextContent, 
                Is.EqualTo(_testForecasts[0].TemperatureC.ToString()));
            Assert.That(firstRow.QuerySelector("td:nth-child(3)")?.TextContent, 
                Is.EqualTo(_testForecasts[0].TemperatureF.ToString()));
            Assert.That(firstRow.QuerySelector("td:nth-child(4)")?.TextContent, 
                Is.EqualTo(_testForecasts[0].Summary));
        });
    }

    [Test]
    public async Task FetchData_WhenServiceReturnsEmptyArray_ShowsEmptyTable()
    {
        // Arrange
        _weatherForecastServiceMock
            .Setup(service => service.GetForecastAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(Array.Empty<WeatherForecast>());

        // Act
        var cut = _testContext.RenderComponent<FetchData>();
        
        // Wait for the component to render and stabilize
        await Task.Delay(50); // Small delay to allow async operation to complete
        cut.Render(); // Force a re-render

        // Assert
        var rows = cut.FindAll("tbody tr");
        Assert.That(rows, Is.Empty);
    }

    [Test]
    public async Task FetchData_VerifyServiceCalledWithCorrectDate()
    {
        // Arrange
        DateTime capturedDate = DateTime.MinValue;
        _weatherForecastServiceMock
            .Setup(service => service.GetForecastAsync(It.IsAny<DateTime>()))
            .Callback<DateTime>(date => capturedDate = date)
            .ReturnsAsync(_testForecasts);

        // Act
        var cut = _testContext.RenderComponent<FetchData>();
        
        // Wait for the component to render and stabilize
        await Task.Delay(50); // Small delay to allow async operation to complete
        cut.Render(); // Force a re-render

        // Assert
        Assert.That(capturedDate.Date, Is.EqualTo(DateTime.Now.Date));
        _weatherForecastServiceMock.Verify(
            service => service.GetForecastAsync(It.IsAny<DateTime>()),
            Times.Once);
    }

    [Test]
    public void FetchData_VerifyTableHeaders()
    {
        // Arrange
        _weatherForecastServiceMock
            .Setup(service => service.GetForecastAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(_testForecasts);

        // Act
        var cut = _testContext.RenderComponent<FetchData>();

        // Assert
        var headers = cut.FindAll("thead th");
        Assert.That(headers, Has.Count.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(headers[0].TextContent, Is.EqualTo("Date"));
            Assert.That(headers[1].TextContent, Is.EqualTo("Temp. (C)"));
            Assert.That(headers[2].TextContent, Is.EqualTo("Temp. (F)"));
            Assert.That(headers[3].TextContent, Is.EqualTo("Summary"));
        });
    }
}
