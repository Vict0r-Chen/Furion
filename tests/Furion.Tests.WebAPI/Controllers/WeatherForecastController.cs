using Furion.Tests.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Furion.Tests.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    public string GetName(ITestService testService
        , ITestExtraService testExtraService
        , TestBaseSerivce testBaseSerivce
        , TestNotInterface testNotInterface
        , ITestGeneric<int, string> testGeneric
        , TestBaseSerivce<int, string> testBaseGeneric
        , TestSingleGeneric<string> c
        , IEnumerable<TestNotInterface> cs
        , TestNotInterface2 testNotInterface2)
    {
        return testService.GetName() + " " + testExtraService.Extra() + " " + testBaseSerivce.GetBase();
    }
}