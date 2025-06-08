using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models
{
    [BsonIgnoreExtraElements]
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        public string Email { get; set; }

        [BsonElement("message")]
        public string Message { get; set; }

        [BsonElement("sentAt")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [BsonElement("type")]
        public string Type { get; set; }
    }
}
