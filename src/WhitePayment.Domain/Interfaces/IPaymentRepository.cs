namespace WhitePayment.Domain.Interface
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetByReferenceAsync(string reference);
    }
}
