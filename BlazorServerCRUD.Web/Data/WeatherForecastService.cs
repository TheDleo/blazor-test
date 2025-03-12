using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerCRUD.Web.Data
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly Dictionary<string, (int MinTemp, int MaxTemp, string[] LikelySummaries)> CityWeatherPatterns = new()
        {
            { "Seattle", (-5, 25, new[] { "Chilly", "Cool", "Mild", "Rainy" }) },
            { "Phoenix", (20, 45, new[] { "Hot", "Sweltering", "Scorching", "Dry" }) },
            { "Chicago", (-15, 35, new[] { "Freezing", "Bracing", "Windy", "Mild" }) },
            { "Miami", (15, 35, new[] { "Humid", "Warm", "Balmy", "Hot" }) }
        };

        public virtual Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }

        public virtual Task<WeatherForecast> GetForecastForCityAsync(string city, DateTime startDate)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City name cannot be empty", nameof(city));

            if (!CityWeatherPatterns.TryGetValue(city, out var pattern))
                throw new KeyNotFoundException($"Weather data not available for city: {city}");

            var rng = new Random();
            var forecast = new WeatherForecast
            {
                Date = startDate,
                TemperatureC = rng.Next(pattern.MinTemp, pattern.MaxTemp),
                Summary = pattern.LikelySummaries[rng.Next(pattern.LikelySummaries.Length)]
            };

            return Task.FromResult(forecast);
        }
    }
}
