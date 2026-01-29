using WhitePayment.Application.Common;

namespace WhitePayment.Application.Interfaces
{
    public interface IWebhookHandlerService
    {
        Task<BaseResponseModel<string>> HandleWebhookAsync(string payload, string signature);
    }
}
