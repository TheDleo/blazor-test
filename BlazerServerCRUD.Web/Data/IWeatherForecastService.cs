using System;
using System.Threading.Tasks;

namespace BlazerServerCRUD.Web.Data
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
    }
}
