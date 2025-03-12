using System;
using System.Threading.Tasks;

namespace BlazorServerCRUD.Web.Data
{
    public interface IWeatherForecastService
    {
        
        Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
        Task<WeatherForecast> GetForecastForCityAsync(string city, DateTime startDate);
    }
}
