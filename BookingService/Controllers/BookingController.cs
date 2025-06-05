using BookingService.Models;
using BookingService.Services;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
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
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            _service.Create(booking);

            await PublishBookingEventAsync(booking.UserId);

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

        private async Task PublishBookingEventAsync(string userId)
        {
            string projectId = "dp2025swd63";
            string topicId = "bookings-topic";

            PublisherServiceApiClient publisher = await PublisherServiceApiClient.CreateAsync();
            TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);

            PubsubMessage message = new PubsubMessage
            {
                Data = ByteString.CopyFromUtf8(userId)
            };

            await publisher.PublishAsync(topicName, new[] { message });
        }
    }
}
