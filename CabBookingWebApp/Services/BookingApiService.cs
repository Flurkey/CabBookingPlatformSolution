using CabBookingWebApp.Models;

namespace CabBookingWebApp.Services
{
    public class BookingApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public BookingApiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _baseUrl = config["Services:BookingApi"]!;
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

        public async Task<bool> MarkBookingAsPaid(string bookingId)
        {
            var res = await _http.PutAsync($"{_baseUrl}/mark-paid/{bookingId}", null);
            return res.IsSuccessStatusCode;
        }

    }
}
