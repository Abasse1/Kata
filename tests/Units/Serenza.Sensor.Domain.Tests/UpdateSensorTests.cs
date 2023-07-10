using Moq;
using Serenza.Sensor.Domain.Abstracttions;
using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Domain.Exceptions;
using Serenza.Sensor.Domain.Models;
using Serenza.Sensor.Domain.Services;

namespace Serenza.Sensor.Domain.Tests;

public class UpdateSensorTests
{
    private readonly SensorHandler _sensorHander;
    private readonly Mock<ISensorRepository> _mockSensorRepository;

    public UpdateSensorTests()
    {
        _mockSensorRepository = new Mock<ISensorRepository>();
        _sensorHander = new SensorHandler(_mockSensorRepository.Object, Mock.Of<ITemperatureCaptorService>());
    }

    [Fact]
    public async Task UpdateSensor_WithNullTemperatureRange_ReturnsFailureResultAsync()
    {
        //Arrange
        int sensorId = 3;
        SensorTemperatureRange? temperatureRange = null;

        //Act
        Result<bool> updateResult = await _sensorHander.UpdateSensorAsync(sensorId, temperatureRange).ConfigureAwait(false);

        //Assert
        _ = Assert.IsType<ArgumentNullException>(updateResult.Exception);
    }

    [Fact]
    public async Task UpdateSensor_WithTemperatureMinGreaterThanTemperatureMax_ReturnFailureUpdateResultAsync()
    {
        //Arrange
        int sensorId = 3;
        SensorTemperatureRange temperatureRange = new(45, null);
        Models.Sensor sensor = Models.Sensor.Create(sensorId, SensorState.WARM, 22, 40);
        _ = _mockSensorRepository.Setup(_ => _.GetSensorByIdAync(sensorId)).ReturnsAsync(sensor);

        //Act
        Result<bool> updateResult = await _sensorHander.UpdateSensorAsync(sensorId, temperatureRange).ConfigureAwait(false);

        //Assert
        Exception exception = Assert.IsType<SensorBusinessException>(updateResult.Exception);
        Assert.Equal(string.Format(SensorBusinessException.TemperatureMinGreaterThanTemperatureMaxErrorMessage
            , sensor.TemperatureMax, temperatureRange.TemperatureMin), exception.Message);
    }

    [Fact]
    public async Task UpdateSensor_WithTemperatureMaxLessThanTemperatureMin_ReturnFailureUpdateResultAsync()
    {
        //Arrange
        int sensorId = 3;
        SensorTemperatureRange temperatureRange = new(null, 20);
        Models.Sensor sensor = Models.Sensor.Create(sensorId, SensorState.WARM, 22, 40);
        _ = _mockSensorRepository.Setup(_ => _.GetSensorByIdAync(sensorId)).ReturnsAsync(sensor);

        //Act
        Result<bool> updateResult = await _sensorHander.UpdateSensorAsync(sensorId, temperatureRange).ConfigureAwait(false);

        //Assert
        Exception exception = Assert.IsType<SensorBusinessException>(updateResult.Exception);
        Assert.Equal(string.Format(SensorBusinessException.TemperatureMinGreaterThanTemperatureMaxErrorMessage
            , temperatureRange.TemperatureMax, sensor.TemperatureMin), exception.Message);
    }

    [Fact]
    public async Task UpdateSensor_WithTemperatureMin_TemperatureMinUpdatedSuccessfullyAsync()
    {
        //Arrange
        int sensorId = 3;
        SensorTemperatureRange temperatureRange = new(19, null);
        Models.Sensor sensor = Models.Sensor.Create(sensorId, SensorState.WARM, 22, 40);
        _ = _mockSensorRepository.Setup(_ => _.GetSensorByIdAync(sensorId)).ReturnsAsync(sensor);

        //Act
        _ = await _sensorHander.UpdateSensorAsync(sensorId, temperatureRange).ConfigureAwait(false);

        //Assert
        Assert.Equal(temperatureRange.TemperatureMin, sensor.TemperatureMin);
    }

    [Fact]
    public async Task UpdateSensor_WithTemperatureMax_TemperatureMaxUpdatedSuccessfullyAsync()
    {
        //Arrange
        int sensorId = 3;
        SensorTemperatureRange temperatureRange = new(null, 37);
        Models.Sensor sensor = Models.Sensor.Create(sensorId, SensorState.WARM, 22, 40);
        _ = _mockSensorRepository.Setup(_ => _.GetSensorByIdAync(sensorId)).ReturnsAsync(sensor);

        //Act
        _ = await _sensorHander.UpdateSensorAsync(sensorId, temperatureRange).ConfigureAwait(false);

        //Assert
        Assert.Equal(temperatureRange.TemperatureMax, sensor.TemperatureMax);
    }
}