namespace Serenza.Sensor.Domain.Models;
public record SensorTemperatureRange(double? TemperatureMin, double? TemperatureMax)
{
    public bool CanUpdateTemperatureRange()
    {
        return TemperatureMax > TemperatureMin;
    }
}