using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Domain.Exceptions;
using Serenza.Sensor.Domain.Models;

namespace Serenza.Sensor.Domain.Tests;

public class SensorTests
{
    [Fact]
    public void CreateSensor_WithTemperatureMinGreaterThanTemperatureMin_ReturnsFailureResult()
    {
        //Arrange
        int temperatureMin = 50;
        int temperatureMax = 49;

        //Act
        Result<Models.Sensor> result = Models.Sensor.Create(1, SensorState.HOT, temperatureMin, temperatureMax);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsFail);
        Assert.Null(result.Value);
        Exception exception = Assert.IsType<SensorBusinessException>(result.Exception);
        Assert.Equal(string.Format(SensorBusinessException.TemperatureMinGreaterThanTemperatureMaxErrorMessage, temperatureMax, temperatureMin), exception.Message);
    }

    [Fact]
    public void CreateSensor_WithSuccess_ReturnsCreatedSensor()
    {
        //Arrange
        int temperatureMin = 25;
        int temperatureMax = 45;

        //Act
        Models.Sensor sensor = Models.Sensor.Create(2, SensorState.WARM, temperatureMin, temperatureMax);

        //Assert
        Assert.NotNull(sensor);
        Assert.Equal(2, sensor.SensorId);
        Assert.Equal(temperatureMax, sensor.TemperatureMax);
        Assert.Equal(temperatureMin, sensor.TemperatureMin);
        Assert.Equal(SensorState.WARM, sensor.State);
    }
}