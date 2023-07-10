using Serenza.Sensor.Domain.Abstracttions;
using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Domain.Models;

namespace Serenza.Sensor.Domain.Services;

public class SensorHandler : ISensorHandler
{
    private readonly ISensorRepository _sensorRepository;
    private readonly ITemperatureCaptorService _temperatureCaptorService;

    public SensorHandler(ISensorRepository sensorRepository, ITemperatureCaptorService temperatureCaptorService)
    {
        _sensorRepository = sensorRepository;
        _temperatureCaptorService = temperatureCaptorService;
    }

    public Task<Result<IEnumerable<SensorHistory>>> GetSensorHistoriesAsync(int? sensorId, int? limit)
    {
        const int DEFAULT_LIMIT = 5;
        if (!limit.HasValue)
        {
            limit = DEFAULT_LIMIT;
        }

        return _sensorRepository.GetSensorHistoriesAsync(sensorId, limit.Value);
    }

    public async Task<Result<SensorState>> GetSensorStateAsync()
    {
        Result<int> temperatureResult = await _temperatureCaptorService.GetTemperatureAsync();
        if (temperatureResult.IsFail)
        {
            return new Result<SensorState>(temperatureResult.Exception!);
        }

        Result<IEnumerable<Models.Sensor>> sensorResult = await _sensorRepository.GetSensorsAync().ConfigureAwait(false);
        if (sensorResult.IsFail)
        {
            return new Result<SensorState>(sensorResult.Exception!);
        }

        Models.Sensor sensor = sensorResult.Value
            !.First(sensor => sensor.TemperatureMin <= temperatureResult
            && sensor.TemperatureMax > temperatureResult);
        _ = await _sensorRepository.CreateSensorHistoryAsync(SensorHistory.Create(sensor.SensorId, temperatureResult, DateTimeOffset.Now)).ConfigureAwait(false);
        return sensor.State;
    }

    public async Task<Result<bool>> UpdateSensorAsync(int sensorId, SensorTemperatureRange temperatureRange)
    {
        if (temperatureRange == null)
        {
            return new Result<bool>(new ArgumentNullException(nameof(temperatureRange)));
        }

        Result<Models.Sensor> sensorResult = await _sensorRepository.GetSensorByIdAync(sensorId).ConfigureAwait(false);
        if (sensorResult.IsFail)
        {
            return new Result<bool>(sensorResult.Exception!);
        }

        Result<Models.Sensor> updatedSensorResult = sensorResult.Value!
            .SetTemperatureMin(temperatureRange.TemperatureMin);
        if (updatedSensorResult.IsFail)
        {
            return new Result<bool>(updatedSensorResult.Exception!);
        }

        updatedSensorResult = updatedSensorResult.Value!
            .SetTemperatureMax(temperatureRange.TemperatureMax);

        if (updatedSensorResult.IsFail)
        {
            return new Result<bool>(updatedSensorResult.Exception!);
        }

        _ = await _sensorRepository.UpdateSensorAsync(updatedSensorResult).ConfigureAwait(false);

        return new Result<bool>(true);
    }
}