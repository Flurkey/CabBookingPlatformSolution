using System.ComponentModel.DataAnnotations;

namespace CabBookingWebApp.Models
{
    public class Location
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Weather { get; set; }
    }
}