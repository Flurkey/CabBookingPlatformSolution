using System;

namespace CabBookingWebApp.Models
{
    public class Payment
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string BookingId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
