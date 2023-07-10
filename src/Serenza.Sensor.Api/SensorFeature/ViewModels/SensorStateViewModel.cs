using Serenza.Sensor.Domain.Enum;

namespace Serenza.Sensor.Api.SensorFeature.ViewModels;

internal record SensorStateViewModel
{
    public SensorState SensorState { get; init; }

    public static SensorStateViewModel Create(SensorState sensorState)
    {
        return new SensorStateViewModel
        {
            SensorState = sensorState
        };
    }
}