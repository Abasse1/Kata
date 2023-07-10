namespace Serenza.Sensor.Domain.Models;

public class SensorHistory
{
    private SensorHistory()
    {
    }

    public int SensorHistoryId { get; private set; }

    public int SensorId { get; private set; }

    public double Temperature { get; private set; }

    public DateTimeOffset TemperatureDate { get; private set; }

    public static SensorHistory Create(int sensorId, double temperature, DateTimeOffset temperatureDate)
    {
        return new SensorHistory
        {
            SensorId = sensorId,
            Temperature = temperature,
            TemperatureDate = temperatureDate
        };
    }

    public static SensorHistory Create(int sensorHistoryId, int sensorId, double temperature, DateTimeOffset temperatureDate)
    {
        return new SensorHistory
        {
            SensorHistoryId = sensorHistoryId,
            SensorId = sensorId,
            Temperature = temperature,
            TemperatureDate = temperatureDate
        };
    }
}