using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Serenza.Sensor.Api.SensorFeature.Extensions;
using Serenza.Sensor.Domain.Abstracttions;
using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Domain.Exceptions;
using Serenza.Sensor.Domain.Models;

namespace Serenza.Sensor.Api.SensorFeature.Controllers;

[ApiController]
[Route("[controller]")]
public class SensorsController : ControllerBase
{
    private readonly ILogger<SensorsController> _logger;
    private readonly ISensorHandler _sensorHandler;

    public SensorsController(ISensorHandler sensorHandler, ILogger<SensorsController> logger)
    {
        _sensorHandler = sensorHandler;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SensorState), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSensorStateAsync()
    {
        Result<SensorState> sensorStateResult = await _sensorHandler.GetSensorStateAsync().ConfigureAwait(false);

        return sensorStateResult.Match<IActionResult>(sensorState =>
        {
            _logger.LogInformation("Sensor state: " + sensorState);
            return Ok(sensorState);
        }, exception =>
        {
            _logger.LogError(exception.Message);
            return StatusCode(500);
        });
    }

    [HttpPatch("{sensorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> UpdateSensorRangeAsync(int sensorId, Operation<SensorTemperatureRange> operation)
    {
        Result<SensorTemperatureRange> sensorTemperatureRangeResult = operation.ToSensorTemperatureRange();

        return sensorTemperatureRangeResult.MatchAsync(async sensorTemperatureRange =>
        {
            Result<bool> updateSensorResult = await _sensorHandler.UpdateSensorAsync(sensorId, sensorTemperatureRange!).ConfigureAwait(false);
            return updateSensorResult.Match<IActionResult>(success =>
            {
                return NoContent();
            }, exception =>
            {
                _logger.LogError(exception.Message);
                return exception is SensorBusinessException
                    ? BadRequest(exception.Message)
                    : exception is SensorNotFoundException ? NotFound(exception.Message) : StatusCode(500);
            });
        }, exception =>
        {
            _logger.LogError(exception.Message);
            return exception is SensorBusinessException ? BadRequest(exception.Message) : StatusCode(500);
        });
    }

    [HttpGet("histories")]
    [ProducesResponseType(typeof(IEnumerable<SensorHistory>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSensorHistoriesAsync(int? sensorId, int? limit)
    {
        if (limit.HasValue && limit.Value <= 0)
        {
            return BadRequest("The limit must be gritter than 0");
        }

        Result<IEnumerable<SensorHistory>> sensorHistoriesResult = await _sensorHandler.GetSensorHistoriesAsync(sensorId, limit).ConfigureAwait(false);
        return sensorHistoriesResult.Match<IActionResult>(Ok, exception =>
        {
            _logger.LogError(exception.Message);
            return StatusCode(500);
        });
    }
}