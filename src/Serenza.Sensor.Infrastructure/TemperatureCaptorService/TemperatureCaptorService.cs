using Serenza.Sensor.Domain.Abstracttions;
using Serenza.Sensor.Domain.Models;

namespace Serenza.Sensor.Infrastructure.TemperatureCaptorService;

public class TemperatureCaptorService : ITemperatureCaptorService
{
    public Task<Result<int>> GetTemperatureAsync()
    {
        return Task.FromResult(new Result<int>(Random.Shared.Next(-20, 70)));
    }
}