using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Domain.Models;
using Serenza.Sensor.Infrastructure.Dto;

namespace Serenza.Sensor.Infrastructure.SensorMapper;

internal static class SensorExtension
{
    internal static Domain.Models.Sensor ToSensor(this SensorDto sensorDto)
    {
        return sensorDto is null ? null : Domain.Models.Sensor.Create(sensorDto.SensorId, Enum.Parse<SensorState>(sensorDto.State),
                sensorDto.TemperatureMin, sensorDto.TemperatureMax)!;
    }

    internal static SensorDto? ToSensorDto(this Domain.Models.Sensor sensor)
    {
        return sensor is null ? null : new SensorDto
        {
            SensorId = sensor.SensorId,
            State = sensor.State.ToString(),
            TemperatureMin = sensor.TemperatureMin,
            TemperatureMax = sensor.TemperatureMax
        };
    }

    internal static SensorHistory? ToSensorHistory(this SensorHistoryDto sensorHistoryDto)
    {
        return sensorHistoryDto is null ? null : SensorHistory.Create(sensorHistoryDto.SensorHistoryId
            , sensorHistoryDto.SensorId, sensorHistoryDto.Temperature, sensorHistoryDto.GetTemperatureDate);
    }

    internal static SensorHistoryDto? ToSensorHistoryDto(this SensorHistory sensorHistory)
    {
        return sensorHistory is null ? null : new SensorHistoryDto
        {
            SensorId = sensorHistory.SensorId,
            Temperature = sensorHistory.Temperature,
            GetTemperatureDate = sensorHistory.TemperatureDate,
        };
    }
}