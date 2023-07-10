using Microsoft.AspNetCore.JsonPatch.Operations;
using Serenza.Sensor.Domain.Exceptions;
using Serenza.Sensor.Domain.Models;

namespace Serenza.Sensor.Api.SensorFeature.Extensions;

public static class UpdateSensorRangeExtensions
{
    private static readonly string PatchWithNoTemeratureRangeErrorMessage = $"Path operation failed: check your patch request ({typeof(SensorTemperatureRange)} is required)";

    internal static Result<SensorTemperatureRange> ToSensorTemperatureRange(this Operation<SensorTemperatureRange> operation)
    {
        SensorTemperatureRange? temperatureRange = operation.value as SensorTemperatureRange;
        if (temperatureRange == null)
        {
            return new Result<SensorTemperatureRange>(new SensorBusinessException(PatchWithNoTemeratureRangeErrorMessage));
        }

        if (temperatureRange is not null && TemperatureMinAndTemperatureMaxHaveValue(temperatureRange))
        {
            if (!temperatureRange.CanUpdateTemperatureRange())
            {
                return new Result<SensorTemperatureRange>(new SensorBusinessException(string.Format(SensorBusinessException.TemperatureMinGreaterThanTemperatureMaxErrorMessage
                        , temperatureRange.TemperatureMax, temperatureRange.TemperatureMin)));
            }
        }
        return temperatureRange!;
    }

    private static bool TemperatureMinAndTemperatureMaxHaveValue(SensorTemperatureRange temperatureRange)
    {
        return temperatureRange.TemperatureMin.HasValue && temperatureRange.TemperatureMax.HasValue;
    }
}