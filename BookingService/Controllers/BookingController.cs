using BookingService.Models;
using BookingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingDbService _service;

        public BookingController(IConfiguration config)
        {
            _service = new BookingDbService(config);
        }

        [HttpPost("create")]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            _service.Create(booking);
            return Ok("Booking created.");
        }

        [HttpGet("current/{userId}")]
        public IActionResult GetCurrent(string userId)
        {
            var bookings = _service.GetCurrentBookings(userId);
            return Ok(bookings);
        }

        [HttpGet("past/{userId}")]
        public IActionResult GetPast(string userId)
        {
            var bookings = _service.GetPastBookings(userId);
            return Ok(bookings);
        }
    }
}
