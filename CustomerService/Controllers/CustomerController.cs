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
        public IActionResult Login([FromBody] Customer customer)
        {
            var existing = _service.GetByEmail(customer.Email);
            if (existing == null)
                return Unauthorized("Invalid credentials.");

            bool isValid = BCrypt.Net.BCrypt.Verify(customer.Password, existing.Password);
            if (!isValid)
                return Unauthorized("Invalid credentials.");

            return Ok("Login successful.");
        }
    }
}
