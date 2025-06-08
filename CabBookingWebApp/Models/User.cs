﻿using System.ComponentModel.DataAnnotations;

namespace CabBookingWebApp.Models
{
    public class User
    {
        public string? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
