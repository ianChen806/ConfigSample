using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConfigSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly MyRootConfig _config;

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MySectionConfig _mySectionConfig;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                         MyRootConfig config,
                                         MySectionConfig mySectionConfig)
        {
            _logger = logger;
            _config = config;
            _mySectionConfig = mySectionConfig;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var mySectionConfig = _mySectionConfig;

            var myRootConfig = _config;

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                             {
                                 Date = DateTime.Now.AddDays(index),
                                 TemperatureC = rng.Next(-20, 55),
                                 Summary = Summaries[rng.Next(Summaries.Length)]
                             })
                             .ToArray();
        }
    }
}