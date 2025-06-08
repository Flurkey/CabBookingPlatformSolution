using CabBookingWebApp.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace CabBookingWebApp.Services
{
    public class CustomerApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public CustomerApiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _baseUrl = config["Services:CustomerApi"]!;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            var response = await _http.PostAsJsonAsync($"{_baseUrl}/register", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<User?> LoginAsync(User user)
        {
            var response = await _http.PostAsJsonAsync($"{_baseUrl}/login", user);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }

            return null;
        }

        public async Task<List<InboxMessage>> GetInboxMessagesAsync(string email)
        {
            var response = await _http.GetAsync($"{_baseUrl}/notifications/{email}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<InboxMessage>>() ?? new List<InboxMessage>();
            }

            return new List<InboxMessage>();
        }

        public async Task<User?> GetProfileAsync(string email)
        {
            var response = await _http.GetAsync($"{_baseUrl}/profile/{email}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }

            return null;
        }
    }
}
