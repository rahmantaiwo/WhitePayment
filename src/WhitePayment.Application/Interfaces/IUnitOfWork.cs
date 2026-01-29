namespace WhitePayment.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
