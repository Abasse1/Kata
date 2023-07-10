using System.Runtime.Serialization;

namespace Serenza.Sensor.Domain.Exceptions;

[Serializable]
public class SensorNotFoundException : Exception
{
    private const string SensorNotFoundMessage = "The sensor {0} is not found.";

    public SensorNotFoundException(int sensorId) : base(string.Format(SensorNotFoundMessage, sensorId))
    {
    }

    public SensorNotFoundException(string? message) : base(message)
    {
    }

    public SensorNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SensorNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}