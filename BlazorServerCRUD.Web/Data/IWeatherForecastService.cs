using System;
using System.Threading.Tasks;

namespace BlazorServerCRUD.Web.Data
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
    }
}
