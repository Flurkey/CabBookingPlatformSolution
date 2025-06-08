using CabBookingWebApp.Models;

namespace CabBookingWebApp.Services
{
    public class BookingApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl = "https://localhost:7284/api/booking";

        public BookingApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            var res = await _http.PostAsJsonAsync($"{_baseUrl}/create", booking);
            return res.IsSuccessStatusCode;
        }

        public async Task<List<Booking>> GetCurrentBookingsAsync(string userId)
        {
            return await _http.GetFromJsonAsync<List<Booking>>($"{_baseUrl}/current/{userId}") ?? new();
        }

        public async Task<List<Booking>> GetPastBookingsAsync(string userId)
        {
            return await _http.GetFromJsonAsync<List<Booking>>($"{_baseUrl}/past/{userId}") ?? new();
        }
    }
}
