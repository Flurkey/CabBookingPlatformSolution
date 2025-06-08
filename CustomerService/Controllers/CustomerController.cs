using CustomerService.Models;
using CustomerService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDbService _service;

        public CustomerController(IConfiguration config)
        {
            _service = new CustomerDbService(config);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Customer customer)
        {
            var existing = _service.GetByEmail(customer.Email);
            if (existing != null)
                return BadRequest("Email already registered.");

            customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            _service.Create(customer);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var existing = _service.GetByEmail(request.Email);
            if (existing == null)
                return Unauthorized("Invalid credentials.");

            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, existing.Password);
            if (!isValid)
                return Unauthorized("Invalid credentials.");

            return Ok(new
            {
                id = existing.Id,
                firstName = existing.FirstName,
                lastName = existing.LastName,
                email = existing.Email
            });
        }

        [HttpGet("profile/{email}")]
        public IActionResult GetProfile(string email)
        {
            var user = _service.GetByEmail(email);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        [HttpGet("notifications/{email}")]
        public IActionResult GetNotifications(string email)
        {
            Console.WriteLine("Controller received email: " + email);
            var notes = _service.GetNotifications(email);
            Console.WriteLine("Found: " + notes.Count + " notifications");
            return Ok(notes);
        }
    }
}
