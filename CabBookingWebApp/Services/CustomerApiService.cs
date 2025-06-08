using CabBookingWebApp.Models;
using System.Net.Http.Json;

namespace CabBookingWebApp.Services
{
    public class CustomerApiService
    {
        private readonly HttpClient _http;

        public CustomerApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            var response = await _http.PostAsJsonAsync("https://localhost:7135/api/customer/register", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<User?> LoginAsync(User user)
        {
            var response = await _http.PostAsJsonAsync("https://localhost:7135/api/customer/login", user);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<User>();
                return data;
            }

            return null;
        }

        public async Task<List<InboxMessage>> GetInboxMessagesAsync(string email)
        {
            var response = await _http.GetAsync($"https://localhost:7135/api/customer/notifications/{email}");

            if (response.IsSuccessStatusCode)
            {
                var messages = await response.Content.ReadFromJsonAsync<List<InboxMessage>>();
                return messages ?? new List<InboxMessage>();
            }

            return new List<InboxMessage>();
        }

        public async Task<User?> GetProfileAsync(string email)
        {
            var response = await _http.GetAsync($"https://localhost:7135/api/customer/profile/{email}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }

            return null;
        }
    }
}
