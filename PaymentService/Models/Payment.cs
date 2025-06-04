using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentService.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string UserId { get; set; }
        public string BookingId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    }
}
