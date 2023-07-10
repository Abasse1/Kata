using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Domain.Exceptions;

namespace Serenza.Sensor.Domain.Models;

public class Sensor
{
    private Sensor()
    {
        SensorHistories = new List<SensorHistory>();
    }

    public int SensorId { get; private set; }

    public SensorState State { get; private set; }

    public double TemperatureMax { get; private set; }

    public double TemperatureMin { get; private set; }

    public IList<SensorHistory> SensorHistories { get; private set; }

    public static Result<Sensor> Create(int sensorId, SensorState sensorState, double temperatureMin, double temperatureMax)
    {
        return IsTemperatureMaxLessThanOrEqualToTemperatureMin(temperatureMin, temperatureMax)
            ? CreateSensorFailedResult(temperatureMin, temperatureMax)
            : new Result<Sensor>(new Sensor
            {
                SensorId = sensorId,
                State = sensorState,
                TemperatureMin = temperatureMin,
                TemperatureMax = temperatureMax
            });
    }

    public Result<Sensor> SetTemperatureMax(double? temepratureMax)
    {
        if (temepratureMax.HasValue)
        {
            if (IsTemperatureMaxLessThanOrEqualToTemperatureMin(TemperatureMin, temepratureMax.Value))
            {
                return CreateSensorFailedResult(TemperatureMin, temepratureMax.Value);
            }

            TemperatureMax = temepratureMax.Value;
        }
        return this;
    }

    public Result<Sensor> SetTemperatureMin(double? temepratureMin)
    {
        if (temepratureMin.HasValue)
        {
            if (IsTemperatureMaxLessThanOrEqualToTemperatureMin(temepratureMin.Value, TemperatureMax))
            {
                return CreateSensorFailedResult(temepratureMin.Value, TemperatureMax);
            }

            TemperatureMin = temepratureMin.Value;
        }
        return this;
    }

    private static bool IsTemperatureMaxLessThanOrEqualToTemperatureMin(double temperatureMin, double temperatureMax)
    {
        return temperatureMax <= temperatureMin;
    }

    private static Result<Sensor> CreateSensorFailedResult(double temperatureMin, double temperatureMax)
    {
        return new Result<Sensor>(
                        new SensorBusinessException(string.Format(SensorBusinessException.TemperatureMinGreaterThanTemperatureMaxErrorMessage
                        , temperatureMax, temperatureMin)));
    }
}