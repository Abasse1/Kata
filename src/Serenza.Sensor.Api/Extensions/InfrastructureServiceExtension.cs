using Serenza.Sensor.Domain.Abstracttions;
using Serenza.Sensor.Infrastructure.Repositories;
using Serenza.Sensor.Infrastructure.TemperatureCaptorService;

namespace Serenza.Sensor.Api.Extensions;

internal static class InfrastructureServiceExtension
{
    internal static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddScoped<ISensorRepository, SensorRepository>()
            .AddSingleton<ITemperatureCaptorService, TemperatureCaptorService>();
    }
}