using Calculator.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<ICalculatorService> CalculatorMock { get; } = new();

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing ICalculatorService registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ICalculatorService));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add the mocked ICalculatorService
            services.AddScoped(_ => CalculatorMock.Object);
        });
    }
}
