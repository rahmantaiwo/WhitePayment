namespace WhitePayment.Application.Common.Options
{
    public class PaystackOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
    }
}