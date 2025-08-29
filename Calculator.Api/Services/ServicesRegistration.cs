using Calculator.Api.Services.Interfaces;

namespace Calculator.Api.Services
{
    public static class ServicesRegistration
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            // Registering the calculator service
            services.AddScoped(typeof(ICalculatorService), typeof(CalculatorService));
            return services;
        }
    }
}
