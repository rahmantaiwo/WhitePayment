using WhitePayment.Application.Common;
using WhitePayment.Application.Dto;

namespace WhitePayment.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<BaseResponseModel<InitializePaymentResponse>> InitializeAsync(InitializePayment dto);
        Task<BaseResponseModel<string>> VerifyAsync(string reference);
    }
}
