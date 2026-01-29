using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WhitePayment.Application.Interfaces;
using WhitePayment.Domain.Interface;
using WhitePayment.Infrastructure.Data;
using WhitePayment.Infrastructure.ExternalServices;
using WhitePayment.Infrastructure.Repositories;

namespace WhitePayment.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Repositories & UnitOfWork
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //External Services
            services.AddHttpClient<IPaymentGateway, PaystackPaymentGateway>(client =>
            {
                client.BaseAddress = new Uri("https://api.paystack.co/");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["Paystack:SecretKey"]}");
            });

            return services;
        }
    }
}
