namespace CabBookingWebApp.Models
{
    public class InboxMessage
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
