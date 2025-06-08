using CabBookingWebApp.Models;
using CabBookingWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CabBookingWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerApiService _service;

        public CustomerController(CustomerApiService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid) return View(user);

            var success = await _service.RegisterAsync(user);
            return success ? RedirectToAction("Login") : View(user);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"{key}: {error.ErrorMessage}");
                    }
                }
                return View(user);
            }

            var loggedInUser = await _service.LoginAsync(user);
            Console.WriteLine($"[LOGIN] userEmail stored in session: {HttpContext.Session.GetString("userEmail")}");

            if (loggedInUser != null)
            {
                HttpContext.Session.SetString("userEmail", loggedInUser.Email);
                return RedirectToAction("Current", "Booking");
            }

            ModelState.AddModelError("", "Invalid login credentials.");
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Inbox()
        {
            var email = HttpContext.Session.GetString("userEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            var user = await _service.GetProfileAsync(email);
            Console.WriteLine("User not found");
            if (user == null) return RedirectToAction("Login", "Customer");

            var inbox = await _service.GetInboxMessagesAsync(user.Email);
            return View(inbox);
        }
    }
}
