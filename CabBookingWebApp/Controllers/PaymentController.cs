using CabBookingWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CabBookingWebApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentApiService _service;

        public PaymentController(PaymentApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var email = HttpContext.Session.GetString("userEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Customer");



            var payments = await _service.GetPaymentsAsync(email);
            return View(payments);
        }
    }

}
