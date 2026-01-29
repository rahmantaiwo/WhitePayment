using Microsoft.Extensions.DependencyInjection;
using WhitePayment.Application.Interfaces;
using WhitePayment.Application.Services;

namespace WhitePayment.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IWebhookHandlerService, WebhookHandlerService>();
            return services;
        }
    }
}
