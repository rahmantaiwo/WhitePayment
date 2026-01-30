namespace WhitePayment.Application.Dto.Webhooks
{
    public class PaystackWebhookEvent
    {
        public string Event { get; set; }
        public PaystackWebhookData Data { get; set; }
    }
}
