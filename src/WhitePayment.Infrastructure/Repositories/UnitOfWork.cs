using WhitePayment.Application.Interfaces;
using WhitePayment.Infrastructure.Data;

namespace WhitePayment.Infrastructure.Repositories
{
    public class UnitOfWork(PaymentDbContext context) : IUnitOfWork
    {
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
