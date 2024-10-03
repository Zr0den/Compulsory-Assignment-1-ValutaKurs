using Microsoft.AspNetCore.Mvc;

namespace SysAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValutaController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ValutaController> _logger;

        public ValutaController(ILogger<ValutaController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<ValutaItems> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new ValutaItems
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


    }
}
