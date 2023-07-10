using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Domain.Models;

namespace Serenza.Sensor.Domain.Abstracttions;

public interface ISensorHandler
{
    Task<Result<SensorState>> GetSensorStateAsync();

    Task<Result<bool>> UpdateSensorAsync(int sensorId, SensorTemperatureRange temperatureRange);

    Task<Result<IEnumerable<SensorHistory>>> GetSensorHistoriesAsync(int? sensorId, int? limit);
}