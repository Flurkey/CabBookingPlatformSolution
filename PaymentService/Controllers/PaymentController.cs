using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Services;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentDbService _service;

        public PaymentController(IConfiguration config)
        {
            _service = new PaymentDbService(config);
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromQuery] string userId, [FromQuery] string bookingId, [FromQuery] string cabType,
                                             [FromQuery] double depLat, [FromQuery] double depLng,
                                             [FromQuery] double arrLat, [FromQuery] double arrLng,
                                             [FromQuery] string bookingTime, [FromQuery] int passengers,
                                             [FromQuery] bool hasDiscount = false)
        {

            if (_service.PaymentExists(bookingId))
                return BadRequest("Payment already processed for this booking.");

            try
            {
                var bookingDate = DateTime.Parse(bookingTime);
                double baseFare = await _service.GetBaseFare(depLat, depLng, arrLat, arrLng);
                double total = _service.CalculateTotal(baseFare, cabType, bookingDate, passengers, hasDiscount);

                var payment = new Payment
                {
                    UserId = userId,
                    BookingId = bookingId,
                    TotalPrice = total
                };

                _service.StorePayment(payment);
                return Ok($"Payment successful. Total: €{total:F2}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Payment failed: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetPayments(string userId)
        {
            var payments = _service.GetPayments(userId);
            return Ok(payments);
        }
    }
}
