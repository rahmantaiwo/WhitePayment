using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using WhitePayment.Application.Common.Options;
using WhitePayment.Application.Dto;
using WhitePayment.Application.Interfaces;
using WhitePayment.Domain.ValueObjects;

namespace WhitePayment.Infrastructure.ExternalServices
{
    public class PaystackPaymentGateway : IPaymentGateway
    {
        private readonly HttpClient _client;
        private readonly PaystackOptions _options;
        public PaystackPaymentGateway(HttpClient client, IOptions<PaystackOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task<InitializePaymentResponse> InitializeAsync(string email, Money amount)
        {
            var payload = new
            {
                email,
                amount = (int)(amount.Value * 100),
                callback_url = _options.CallbackUrl
            };


            var response = await _client.PostAsJsonAsync("transaction/initialize", payload);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            var data = json.GetProperty("data");

            return new InitializePaymentResponse
            {
                AuthorizationUrl = data.GetProperty("authorization_url").GetString()!,
                Reference = data.GetProperty("reference").GetString()!
            };
        }

        public async Task<bool> VerifyAsync(string reference)
        {
            var response = await _client.GetAsync($"transaction/verify/{reference}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            return json.GetProperty("data")
                       .GetProperty("status")
                       .GetString() == "success";
        }
    }
}
