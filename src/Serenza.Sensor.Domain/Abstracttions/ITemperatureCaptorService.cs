using Serenza.Sensor.Domain.Models;

namespace Serenza.Sensor.Domain.Abstracttions;

public interface ITemperatureCaptorService
{
    Task<Result<int>> GetTemperatureAsync();
}