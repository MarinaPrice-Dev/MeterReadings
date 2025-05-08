namespace MeterReadings.Services;

using Models;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> GetForecast();
}
