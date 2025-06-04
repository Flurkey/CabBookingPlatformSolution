using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService.Models
{
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("startLocation")]
        public string StartLocation { get; set; }

        [BsonElement("endLocation")]
        public string EndLocation { get; set; }

        [BsonElement("bookingTime")]
        public DateTime BookingTime { get; set; }

        [BsonElement("passengers")]
        public int Passengers { get; set; }

        [BsonElement("cabType")]
        public string CabType { get; set; }
    }
}
