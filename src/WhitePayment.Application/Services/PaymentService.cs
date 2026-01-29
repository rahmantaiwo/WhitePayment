using Microsoft.Extensions.Logging;
using WhitePayment.Application.Common;
using WhitePayment.Application.Dto;
using WhitePayment.Application.Interfaces;
using WhitePayment.Domain;
using WhitePayment.Domain.Interface;
using WhitePayment.Domain.ValueObjects;

namespace WhitePayment.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentGateway _gateway;
        private readonly IPaymentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentService> _logger;

        // Primary constructor injection
        public PaymentService(
            IPaymentGateway gateway,
            IPaymentRepository repository,
            IUnitOfWork unitOfWork,
            ILogger<PaymentService> logger)
        {
            _gateway = gateway;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponseModel<InitializePaymentResponse>> InitializeAsync(InitializePayment dto)
        {
            try
            {
                var amount = new Money(dto.Amount);

                var paymentResponse = await _gateway.InitializeAsync(dto.Email, amount);

                var payment = new Payment(paymentResponse.Reference, dto.Email, amount);

                await _repository.AddAsync(payment);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Payment initialized successfully. Reference: {Reference}", paymentResponse.Reference);

                return BaseResponseModel<InitializePaymentResponse>.Success( paymentResponse, "Payment initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize payment for email {Email}", dto.Email);
                return BaseResponseModel<InitializePaymentResponse>.Failure("Failed to initialize payment");
            }
        }

        public async Task<BaseResponseModel<string>> VerifyAsync(string reference)
        {
            try
            {
                var isSuccess = await _gateway.VerifyAsync(reference);

                var payment = await _repository.GetByReferenceAsync(reference);
                if (payment == null)
                {
                    _logger.LogWarning("Payment not found for reference {Reference}", reference);
                    return BaseResponseModel<string>.Failure("Payment not found", 404);
                }

                if (isSuccess)
                    payment.MarkSuccessful();
                else
                    payment.MarkFailed();

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Payment verification completed. Reference: {Reference}, Success: {Success}", reference, isSuccess);

                return BaseResponseModel<string>.Success(reference, "Payment verified successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify payment for reference {Reference}", reference);
                return BaseResponseModel<string>.Failure("Failed to verify payment");
            }
        }
    }
}
