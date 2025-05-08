using Microsoft.AspNetCore.Mvc;
using MeterReadings.Services;
using MeterReadings.Models;

namespace MeterReadings.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastController(IWeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        var result = _service.GetForecast();
        return Ok(result);
    }
}