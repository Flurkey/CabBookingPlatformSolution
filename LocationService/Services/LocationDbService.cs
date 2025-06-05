using LocationService.Models;
using MongoDB.Driver;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace LocationService.Services
{
    public class LocationDbService
    {
        private readonly IMongoCollection<Location> _locations;
        private readonly string _apiKey;
        private readonly string _apiBase;

        public LocationDbService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _locations = database.GetCollection<Location>(config["MongoDB:LocationCollectionName"]);

            _apiKey = config["WeatherAPI:Key"];
            _apiBase = config["WeatherAPI:BaseUrl"];
        }

        public List<Location> GetByUser(string userId) => _locations.Find(l => l.UserId == userId).ToList();

        public void Add(Location loc) => _locations.InsertOne(loc);

        public void Update(string id, Location loc) => _locations.ReplaceOne(l => l.Id == id, loc);

        public void Delete(string id) => _locations.DeleteOne(l => l.Id == id);

        public async Task<string> GetWeather(string city)
        {
            var client = new RestClient(_apiBase);
            var request = new RestRequest("current.json", Method.Get);
            request.AddParameter("q", city);
            request.AddHeader("X-RapidAPI-Key", _apiKey);
            request.AddHeader("X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com");

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new Exception("Failed to get weather");

            JObject result = JObject.Parse(response.Content);
            string condition = result["current"]?["condition"]?["text"]?.ToString() ?? "Unavailable";
            string temp = result["current"]?["temp_c"]?.ToString() ?? "?";

            return $"{condition}, {temp}°C";
        }
    }
}
