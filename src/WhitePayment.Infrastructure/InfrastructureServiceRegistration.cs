using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using WhitePayment.Application.Common.Options;
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
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Configure<PaystackOptions>(
                configuration.GetSection("Paystack"));

            services.AddHttpClient<IPaymentGateway, PaystackPaymentGateway>()
                .ConfigureHttpClient((sp, client) =>
                {
                    var options = sp
                        .GetRequiredService<IOptions<PaystackOptions>>()
                        .Value;

                    client.BaseAddress = new Uri(options.BaseUrl);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", options.SecretKey);
                });

            return services;
        }

    }
}
