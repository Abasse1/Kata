using Serenza.Sensor.Domain.Models;

namespace Serenza.Sensor.Domain.Abstracttions;

public interface ISensorRepository
{
    Task<Result<Models.Sensor>> UpdateSensorAsync(Models.Sensor sensor);

    Task<Result<IEnumerable<Models.Sensor>>> GetSensorsAync();

    Task<Result<Models.Sensor>> GetSensorByIdAync(int sensorId);

    Task<Result<SensorHistory>> CreateSensorHistoryAsync(SensorHistory sensorHistory);

    Task<Result<IEnumerable<SensorHistory>>> GetSensorHistoriesAsync(int? sensorId, int limit);
}