using Moq;
using Serenza.Sensor.Domain.Abstracttions;
using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Domain.Models;
using Serenza.Sensor.Domain.Services;

namespace Serenza.Sensor.Domain.Tests;

public class GetSensorStateTests
{
    private readonly SensorHandler _sensorHander;
    private readonly Mock<ISensorRepository> _mockSensorRepository;
    private readonly Mock<ITemperatureCaptorService> _mockTemperatureCaptorService;

    public GetSensorStateTests()
    {
        _mockSensorRepository = new Mock<ISensorRepository>();
        _mockTemperatureCaptorService = new Mock<ITemperatureCaptorService>();
        _sensorHander = new SensorHandler(_mockSensorRepository.Object, _mockTemperatureCaptorService.Object);
    }

    [Fact]
    public async Task GetSensorState_WithFailureGetTemperature_ReturnsSensorFailureResultAsync()
    {
        //Arrange
        Exception expectedFailException = new("Failed to get temperature");
        _ = _mockTemperatureCaptorService.Setup(_ => _.GetTemperatureAsync())
            .ReturnsAsync(new Models.Result<int>(expectedFailException));

        //Act
        Models.Result<SensorState> sensorResult = await _sensorHander.GetSensorStateAsync().ConfigureAwait(false);

        //Assert
        Assert.NotNull(sensorResult.Exception);
        Assert.Equal(expectedFailException, sensorResult.Exception);
    }

    [Fact]
    public async Task GetSensorState_WithFailureGetSensors_ReturnsSensorFailureResultAsync()
    {
        //Arrange
        Exception expectedFailException = new("Failed to get sensors");
        _ = _mockTemperatureCaptorService.Setup(_ => _.GetTemperatureAsync())
            .ReturnsAsync(40);
        _ = _mockSensorRepository.Setup(_ => _.GetSensorsAync())
            .ReturnsAsync(new Result<IEnumerable<Models.Sensor>>(expectedFailException));

        //Act
        Models.Result<SensorState> sensorResult = await _sensorHander.GetSensorStateAsync().ConfigureAwait(false);

        //Assert
        Assert.NotNull(sensorResult.Exception);
        Assert.Equal(expectedFailException, sensorResult.Exception);
        _mockSensorRepository.Verify(_ => _.CreateSensorHistoryAsync(It.IsAny<SensorHistory>()), Times.Never());
    }

    [Fact]
    public async Task GetSensorState_WithTemperatureGreaterOrEqualThan40_ReturnHOT()
    {
        //Arrange
        int temperature = 55;
        List<Models.Sensor> sensors = new()
        {
            Models.Sensor.Create(1, SensorState.HOT, 40, short.MaxValue),
            Models.Sensor.Create(2, SensorState.COLD, short.MinValue, 22),
            Models.Sensor.Create(3, SensorState.COLD, 22, 40) };
        _ = _mockSensorRepository.Setup(_ => _.GetSensorsAync())
            .ReturnsAsync(sensors);
        _ = _mockTemperatureCaptorService.Setup(_ => _.GetTemperatureAsync())
            .ReturnsAsync(temperature);
        SensorState expectedState = SensorState.HOT;

        //Act
        SensorState actualState = await _sensorHander.GetSensorStateAsync().ConfigureAwait(false);

        //Assert
        Assert.Equal(expectedState, actualState);
        _mockSensorRepository.Verify(_ => _.CreateSensorHistoryAsync(It.IsAny<SensorHistory>()), Times.Once());
    }

    [Fact]
    public async Task GetSensorState_WithTemperatureLessThan22_ReturnCOLDAsync()
    {
        //Arrange
        int temperature = 10;
        List<Models.Sensor> sensors = new()
        {
            Models.Sensor.Create(1, SensorState.HOT, 40, short.MaxValue),
            Models.Sensor.Create(2, SensorState.COLD, short.MinValue, 22),
            Models.Sensor.Create(3, SensorState.COLD, 22, 40) };
        _ = _mockSensorRepository.Setup(_ => _.GetSensorsAync())
            .ReturnsAsync(sensors);
        _ = _mockTemperatureCaptorService.Setup(_ => _.GetTemperatureAsync())
            .ReturnsAsync(temperature);
        SensorState expectedState = SensorState.COLD;

        //Act
        SensorState actualState = await _sensorHander.GetSensorStateAsync().ConfigureAwait(false);

        //Assert
        Assert.Equal(expectedState, actualState);
        _mockSensorRepository.Verify(_ => _.CreateSensorHistoryAsync(It.IsAny<SensorHistory>()), Times.Once());
    }

    [Fact]
    public async Task GetSensorState_WithTemperatureBetween22And40_ReturnWARMAsync()
    {
        //Arrange
        int temperature = 30;
        List<Models.Sensor> sensors = new()
        {
            Models.Sensor.Create(1, SensorState.HOT, 40, short.MaxValue),
            Models.Sensor.Create(2, SensorState.COLD, short.MinValue, 22),
            Models.Sensor.Create(3, SensorState.WARM, 22, 40) };
        _ = _mockSensorRepository.Setup(_ => _.GetSensorsAync())
            .ReturnsAsync(sensors);
        _ = _mockTemperatureCaptorService.Setup(_ => _.GetTemperatureAsync())
            .ReturnsAsync(temperature);
        SensorState expectedState = SensorState.WARM;

        //Act
        SensorState actualState = await _sensorHander.GetSensorStateAsync().ConfigureAwait(false);

        //Assert
        Assert.Equal(expectedState, actualState);
        _mockSensorRepository.Verify(_ => _.CreateSensorHistoryAsync(It.IsAny<SensorHistory>()), Times.Once());
    }

    [Theory]
    [InlineData(35, SensorState.HOT)]
    [InlineData(199, SensorState.HOT)]
    [InlineData(-10, SensorState.COLD)]
    [InlineData(19, SensorState.COLD)]
    [InlineData(10, SensorState.COLD)]
    [InlineData(20, SensorState.WARM)]
    [InlineData(34, SensorState.WARM)]
    public async Task GetSensorState_WithTemperature_ReturnCorrespondingState(int temperature, SensorState expectedState)
    {
        //Arrange
        List<Models.Sensor> sensors = new()
        {
            Models.Sensor.Create(1, SensorState.HOT, 35, 200),
            Models.Sensor.Create(2, SensorState.COLD, -15, 20),
            Models.Sensor.Create(3, SensorState.WARM, 20, 35) };
        _ = _mockSensorRepository.Setup(_ => _.GetSensorsAync())
            .ReturnsAsync(sensors);
        _ = _mockTemperatureCaptorService.Setup(_ => _.GetTemperatureAsync())
            .ReturnsAsync(temperature);

        //Act
        SensorState actualState = await _sensorHander.GetSensorStateAsync().ConfigureAwait(false);

        //Assert
        Assert.Equal(expectedState, actualState);
        _mockSensorRepository.Verify(_ => _.CreateSensorHistoryAsync(It.IsAny<SensorHistory>()), Times.Once());
    }
}