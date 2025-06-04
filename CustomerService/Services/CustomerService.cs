using CustomerService.Models;
using MongoDB.Driver;

namespace CustomerService.Services
{
    public class CustomerDbService
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerDbService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _customers = database.GetCollection<Customer>(config["MongoDB:CustomerCollectionName"]);
        }

        public List<Customer> GetAll() => _customers.Find(_ => true).ToList();

        public Customer GetByEmail(string email) => _customers.Find(c => c.Email == email).FirstOrDefault();

        public void Create(Customer customer) => _customers.InsertOne(customer);
    }
}
