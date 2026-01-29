using WhitePayment.Application.Dto;
using WhitePayment.Domain.ValueObjects;

namespace WhitePayment.Application.Interfaces
{
    public interface IPaymentGateway
    {
        Task<InitializePaymentResponse> InitializeAsync(string email, Money amount);
        Task<bool> VerifyAsync(string reference);
    }
}
