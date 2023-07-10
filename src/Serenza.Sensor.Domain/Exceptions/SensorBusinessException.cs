using System.Runtime.Serialization;

namespace Serenza.Sensor.Domain.Exceptions;

[Serializable]
public class SensorBusinessException : Exception
{
    public const string TemperatureMinGreaterThanTemperatureMaxErrorMessage = "The temperature max {0} must be greater than the temperature min {1}";

    public SensorBusinessException()

    {
    }

    public SensorBusinessException(string? message) : base(message)
    {
    }

    public SensorBusinessException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SensorBusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}