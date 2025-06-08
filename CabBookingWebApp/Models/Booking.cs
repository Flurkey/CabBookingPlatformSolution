using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CabBookingWebApp.Models
{
    public class Booking
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime BookingTime { get; set; }
        [JsonPropertyName("passengers")]
        public int PassengerCount { get; set; }
        public string CabType { get; set; }
        public bool IsPaid { get; set; }
    }
}
