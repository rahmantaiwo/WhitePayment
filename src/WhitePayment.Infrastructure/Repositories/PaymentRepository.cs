using Microsoft.EntityFrameworkCore;
using WhitePayment.Domain;
using WhitePayment.Domain.Interface;
using WhitePayment.Infrastructure.Data;

namespace WhitePayment.Infrastructure.Repositories
{
    public class PaymentRepository(PaymentDbContext context) : IPaymentRepository
    {
        public async Task AddAsync(Payment payment)
        {
            await context.Payments.AddAsync(payment);
        }

        public async Task<Payment?> GetByReferenceAsync(string reference)
        {
           return await context.Payments.Where(p => p.Reference == reference).FirstOrDefaultAsync();
        }
    }
}
