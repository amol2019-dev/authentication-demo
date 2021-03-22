using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvikEnterprises.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdvikEnterprises.Controllers
{
    [JwtFilterFactory]
    [ApiController]
    [Route("[controller]")]
    //[AutoValidateAntiforgeryToken]
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
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [JwtFilterFactory(Roles = "User")]
        public WeatherForecast GetUserData()
        {
            return new WeatherForecast() {
                Date = DateTime.Now.AddDays(0),
                TemperatureC = 10,
                Summary = "summary test"
            };
        }

        [HttpGet("[action]")]
        [JwtFilterFactory(Roles = "User")]
        public IEnumerable<WeatherForecast> GetUser()
        {
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
