using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WhitePayment.Infrastructure.Data
{
    public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{environment}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection not found.");

            var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
            optionsBuilder.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName)
            );

            return new PaymentDbContext(optionsBuilder.Options);
        }
    }

}
