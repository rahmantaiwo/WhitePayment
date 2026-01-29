namespace WhitePayment.Application.Dto
{
    public class InitializePaymentResponse
    {
        public string AuthorizationUrl { get; set; }
        public string Reference { get; set; }
    }
}
