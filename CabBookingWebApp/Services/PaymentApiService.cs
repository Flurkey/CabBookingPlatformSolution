using CabBookingWebApp.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace CabBookingWebApp.Services
{
    public class PaymentApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public PaymentApiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _baseUrl = config["Services:PaymentApi"]!;
        }

        public async Task<string> PayAsync(string userId, string bookingId, string cabType,
                                           double depLat, double depLng, double arrLat, double arrLng,
                                           DateTime bookingTime, int passengers, bool hasDiscount)
        {
            var query = $"?userId={userId}" +
                        $"&bookingId={bookingId}" +
                        $"&cabType={cabType}" +
                        $"&depLat={depLat}" +
                        $"&depLng={depLng}" +
                        $"&arrLat={arrLat}" +
                        $"&arrLng={arrLng}" +
                        $"&bookingTime={bookingTime:o}" +
                        $"&passengers={passengers}" +
                        $"&hasDiscount={hasDiscount.ToString().ToLower()}";

            var res = await _http.PostAsync($"{_baseUrl}/pay{query}", null);
            return await res.Content.ReadAsStringAsync();
        }

        public async Task<List<Payment>> GetPaymentsAsync(string userId)
        {
            return await _http.GetFromJsonAsync<List<Payment>>($"{_baseUrl}/{userId}") ?? new();
        }
    }
}
