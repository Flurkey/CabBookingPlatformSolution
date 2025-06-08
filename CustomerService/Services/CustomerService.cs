using CustomerService.Models;
using MongoDB.Driver;

namespace CustomerService.Services
{
    public class CustomerDbService
    {
        private readonly IMongoCollection<Customer> _customers;
        private readonly IMongoCollection<Notification> _notifications;

        public CustomerDbService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);

            _customers = database.GetCollection<Customer>(config["MongoDB:CustomerCollectionName"]);
            _notifications = database.GetCollection<Notification>("Inbox");
        }

        public List<Customer> GetAll() => _customers.Find(_ => true).ToList();

        public Customer GetByEmail(string email) => _customers.Find(c => c.Email == email).FirstOrDefault();

        public void Create(Customer customer) => _customers.InsertOne(customer);

        public List<Notification> GetNotifications(string email)
        {
            var filter = Builders<Notification>.Filter.Eq("userId", email.ToLower());
            return _notifications.Find(filter).SortByDescending(n => n.Timestamp).ToList();
        }

        public void AddNotification(string email, string message)
        {
            var notification = new Notification
            {
                Email = email.ToLower(),
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            _notifications.InsertOne(notification);
        }
    }
}
