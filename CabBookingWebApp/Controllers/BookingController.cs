using CabBookingWebApp.Models;
using CabBookingWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CabBookingWebApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingApiService _service;

        private readonly CustomerApiService _customerService;

        private readonly PaymentApiService _paymentService;

        public BookingController(BookingApiService service, CustomerApiService customerService, PaymentApiService paymentService)
        {
            _service = service;
            _customerService = customerService;
            _paymentService = paymentService;
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            var userEmail = HttpContext.Session.GetString("userEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login", "Customer");

            booking.UserId = userEmail;

            var success = await _service.CreateBookingAsync(booking);
            if (success)
                return RedirectToAction("Current");

            ModelState.AddModelError("", "Booking failed.");
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(string bookingId)
        {
            var email = HttpContext.Session.GetString("userEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Customer");

            var bookings = await _service.GetCurrentBookingsAsync(email);
            var booking = bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null) return NotFound();

            double depLat = 35.9, depLng = 14.5;
            double arrLat = 35.91, arrLng = 14.51;

            bool hasDiscount = bookings.Count(b => b.IsPaid) >= 3;

            var result = await _paymentService.PayAsync(
                 email,
                 bookingId,
                 booking.CabType,
                 depLat,
                 depLng,
                 arrLat,
                 arrLng,
                 booking.BookingTime,
                 booking.PassengerCount,
                 hasDiscount
            );

            if (result.StartsWith("Payment successful"))
            {
                // ✅ Mark booking as paid
                await _service.MarkBookingAsPaid(bookingId);
            }

            TempData["PayMessage"] = result;
            return RedirectToAction("Current");
        }


        public async Task<IActionResult> Current()
        {
            var email = HttpContext.Session.GetString("userEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Customer");

            var user = await _customerService.GetProfileAsync(email);

            if (user == null) { Console.WriteLine("User not found"); }
            if (user == null) return RedirectToAction("Login", "Customer");

            var bookings = await _service.GetCurrentBookingsAsync(user.Email);
            return View(bookings);
        }

        public async Task<IActionResult> Past()
        {
            var email = HttpContext.Session.GetString("userEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Customer");

            var user = await _customerService.GetProfileAsync(email);
            Console.WriteLine("User not found");
            if (user == null) return RedirectToAction("Login", "Customer");

            var bookings = await _service.GetPastBookingsAsync(user.Email);
            return View(bookings);
        }
    }
}
