using BookingService.Models;
using MongoDB.Driver;

namespace BookingService.Services
{
    public class BookingDbService
    {
        private readonly IMongoCollection<Booking> _bookings;

        public BookingDbService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _bookings = database.GetCollection<Booking>(config["MongoDB:BookingCollectionName"]);
        }

        public void Create(Booking booking) => _bookings.InsertOne(booking);

        public List<Booking> GetCurrentBookings(string userId) =>
            _bookings.Find(b => b.UserId == userId && b.BookingTime >= DateTime.UtcNow).ToList();

        public List<Booking> GetPastBookings(string userId) =>
            _bookings.Find(b => b.UserId == userId && b.BookingTime < DateTime.UtcNow).ToList();
    }
}