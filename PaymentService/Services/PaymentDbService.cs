using MongoDB.Driver;
using PaymentService.Models;
using RestSharp;

namespace PaymentService.Services
{
    public class PaymentDbService
    {
        private readonly IMongoCollection<Payment> _payments;
        private readonly string _rapidApiKey;
        private readonly string _rapidApiBase;

        public PaymentDbService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _payments = database.GetCollection<Payment>(config["MongoDB:PaymentCollectionName"]);

            _rapidApiKey = config["RapidAPI:Key"];
            _rapidApiBase = config["RapidAPI:BaseUrl"];
        }

        public async Task<double> GetBaseFare(double depLat, double depLng, double arrLat, double arrLng)
        {
            var client = new RestClient(_rapidApiBase);
            var request = new RestRequest("/search-geo", Method.Get);
            request.AddParameter("dep_lat", depLat);
            request.AddParameter("dep_lng", depLng);
            request.AddParameter("arr_lat", arrLat);
            request.AddParameter("arr_lng", arrLng);

            request.AddHeader("X-RapidAPI-Key", _rapidApiKey);
            request.AddHeader("X-RapidAPI-Host", "taxi-fare-calculator.p.rapidapi.com");

            var response = await client.ExecuteAsync(request);

            Console.WriteLine("API Response Content: " + response.Content);

            if (!response.IsSuccessful)
                throw new Exception("Failed to get fare from external API.");

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            double priceInCents = result.journey.fares[0].price_in_cents;
            double fare = priceInCents / 100.0;
            return fare;
        }

        public double CalculateTotal(double baseFare, string cabType, DateTime bookingTime, int passengers, bool hasDiscount)
        {
            double cabMultiplier = cabType switch
            {
                "Economic" => 1.0,
                "Premium" => 1.2,
                "Executive" => 1.4,
                _ => 1.0
            };

            double timeMultiplier = (bookingTime.TimeOfDay >= TimeSpan.FromHours(0) && bookingTime.TimeOfDay < TimeSpan.FromHours(8)) ? 1.2 : 1.0;

            double passengerMultiplier = passengers switch
            {
                <= 4 => 1.0,
                <= 8 => 2.0,
                > 8 => throw new Exception("Too many passengers"),
            };

            double discountMultiplier = hasDiscount ? 0.9 : 1.0;

            return baseFare * cabMultiplier * timeMultiplier * passengerMultiplier * discountMultiplier;
        }

        public void StorePayment(Payment payment) => _payments.InsertOne(payment);

        public List<Payment> GetPayments(string userId) => _payments.Find(p => p.UserId == userId).ToList();
    }
}
