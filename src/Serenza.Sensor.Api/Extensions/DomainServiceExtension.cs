using Serenza.Sensor.Domain.Abstracttions;
using Serenza.Sensor.Domain.Services;

namespace Serenza.Sensor.Api.Extensions;

internal static class DomainServiceExtension
{
    internal static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services.AddScoped<ISensorHandler, SensorHandler>();
    }
}