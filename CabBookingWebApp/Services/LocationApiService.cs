using CabBookingWebApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.WebRequestMethods;

namespace CabBookingWebApp.Services
{
    public class LocationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7157/api/Location";

        public LocationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Location>> GetLocationsAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{userId}");
            if (!response.IsSuccessStatusCode) return new List<Location>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Location>>(json) ?? new List<Location>();
        }

        public async Task AddLocationAsync(Location location)
        {
            var json = JsonConvert.SerializeObject(location);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(_baseUrl, content);
        }

        public async Task DeleteLocationAsync(string id)
        {
            await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/weather/{city}");
            if (!response.IsSuccessStatusCode) return "Unable to retrieve weather.";

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Location> GetByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Location>();
        }

        public async Task UpdateAsync(string id, Location updated)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", updated);
            response.EnsureSuccessStatusCode();
        }
    }
}
