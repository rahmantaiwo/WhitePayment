using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WhitePayment.Application.Common;
using WhitePayment.Application.Dto.Webhooks;
using WhitePayment.Application.Interfaces;
using WhitePayment.Application.Common.Options;

namespace WhitePayment.Application.Services
{
    public class WebhookHandlerService : IWebhookHandlerService
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<WebhookHandlerService> _logger;
        private readonly string _paystackSecretKey;

        public WebhookHandlerService(
            IPaymentService paymentService,
            ILogger<WebhookHandlerService> logger,
            IOptions<PaystackOptions> options)
        {
            _paymentService = paymentService;
            _logger = logger;
            _paystackSecretKey = options.Value.SecretKey;
        }

        public async Task<BaseResponseModel<string>> HandleWebhookAsync(
            string payload,
            string signatureHeader)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signatureHeader))
                    return BaseResponseModel<string>.Failure("Missing signature", 400);

                var computedHash = ComputeSha512Hash(payload, _paystackSecretKey);
                if (!string.Equals(computedHash, signatureHeader, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Invalid Paystack webhook signature");
                    return BaseResponseModel<string>.Failure("Invalid signature", 401);
                }

                var webhookEvent = JsonSerializer.Deserialize<PaystackWebhookEvent>(payload);
                if (webhookEvent?.Data?.Reference == null)
                    return BaseResponseModel<string>.Failure("Invalid webhook payload", 400);

                _logger.LogInformation(
                    "Webhook received: {Event} - Reference: {Reference}",
                    webhookEvent.Event,
                    webhookEvent.Data.Reference);

                await _paymentService.VerifyAsync(webhookEvent.Data.Reference);

                return BaseResponseModel<string>
                    .Success("OK", "Webhook processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Paystack webhook");
                return BaseResponseModel<string>
                    .Failure("Webhook processing failed", 500);
            }
        }

        private static string ComputeSha512Hash(string payload, string secret)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
