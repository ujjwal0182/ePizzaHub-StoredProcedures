using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Core.Settings;
using Microsoft.Extensions.Options;

namespace ePizzaHub.API.Services
{
    public class RazorpayGateway : IPaymentGateway
    {
        private readonly HttpClient _http;

        public RazorpayGateway(HttpClient http, IOptions<RazorpaySettings> options)
        {
            var s = options.Value;
            _http = http;
            _http.BaseAddress = new Uri("https://api.razorpay.com/v1/");
            var basic = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{s.KeyId}:{s.KeySecret}"));
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basic);
        }

        public async Task<GatewayOrder> CreateOrderAsync(long amountInPaise, string currency, string receipt)
        {
            var response = await _http.PostAsJsonAsync("orders", new { amount = amountInPaise, currency, receipt });
            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = doc.RootElement;
            return new GatewayOrder(
                root.GetProperty("id").GetString()!,
                root.GetProperty("amount").GetInt64(),
                root.GetProperty("currency").GetString()!);
        }

        public async Task<GatewayPayment> GetPaymentAsync(string paymentId)
        {
            var response = await _http.GetAsync($"payments/{Uri.EscapeDataString(paymentId)}");
            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = doc.RootElement;
            return new GatewayPayment(
                root.GetProperty("id").GetString()!,
                root.GetProperty("order_id").GetString() ?? string.Empty,
                root.GetProperty("status").GetString()!,
                root.GetProperty("amount").GetInt64(),
                root.GetProperty("currency").GetString()!);
        }
    }
}