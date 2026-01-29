using System.Net.Http.Json;
using System.Text.Json;
using WhitePayment.Application.Dto;
using WhitePayment.Application.Interfaces;
using WhitePayment.Domain.ValueObjects;

namespace WhitePayment.Infrastructure.ExternalServices
{
    public class PaystackPaymentGateway : IPaymentGateway
    {
        private readonly HttpClient _client;

        public PaystackPaymentGateway(HttpClient client) => _client = client;

        public async Task<InitializePaymentResponse> InitializeAsync(string email, Money amount)
        {
            var payload = new { email, amount = (int)(amount.Amount * 100) };
            var response = await _client.PostAsJsonAsync("transaction/initialize", payload);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var data = json.GetProperty("data");

            return new InitializePaymentResponse
            {
                AuthorizationUrl = data.GetProperty("authorization_url").GetString(),
                Reference = data.GetProperty("reference").GetString()
            };
        }

        public async Task<bool> VerifyAsync(string reference)
        {
            var response = await _client.GetAsync($"transaction/verify/{reference}");
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("data").GetProperty("status").GetString() == "success";
        }
    }
}
