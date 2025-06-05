using Microsoft.AspNetCore.Mvc;
using LocationService.Models;
using LocationService.Services;

namespace LocationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly LocationDbService _service;

        public LocationController(IConfiguration config)
        {
            _service = new LocationDbService(config);
        }

        [HttpGet("{userId}")]
        public IActionResult GetLocations(string userId)
        {
            var list = _service.GetByUser(userId);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Add(Location location)
        {
            _service.Add(location);
            return Ok("Location added.");
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Location updated)
        {
            _service.Update(id, updated);
            return Ok("Location updated.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _service.Delete(id);
            return Ok("Location deleted.");
        }

        [HttpGet("weather/{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                var weather = await _service.GetWeather(city);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
