using Microsoft.EntityFrameworkCore;
using Serenza.Sensor.Infrastructure.Dto;
using Serenza.Sensor.Infrastructure.Repositories;

namespace Serenza.Sensor.Api.Extensions;

internal static class ApiServiceExtension
{
    internal static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<SensorSqlitDbContext>(options =>
        {
            _ = options.UseSqlite(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Serenza.Sensor.Api"));
        }).Configure<List<SensorDto>>(configuration.GetSection("SensorsInitial"))
        .AddRouting(options => options.LowercaseUrls = true);
    }
}