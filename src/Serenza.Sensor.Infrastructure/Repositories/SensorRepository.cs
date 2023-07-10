using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serenza.Sensor.Domain.Abstracttions;
using Serenza.Sensor.Domain.Exceptions;
using Serenza.Sensor.Domain.Models;
using Serenza.Sensor.Infrastructure.Dto;
using Serenza.Sensor.Infrastructure.SensorMapper;

namespace Serenza.Sensor.Infrastructure.Repositories;

public class SensorRepository : ISensorRepository
{
    protected readonly SensorSqlitDbContext _dbContext;

    public SensorRepository(SensorSqlitDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Domain.Models.Sensor>> GetSensorByIdAync(int sensorId)
    {
        try
        {
            SensorDto? sensorDto = await _dbContext.Sensors.FindAsync(sensorId).ConfigureAwait(false);
            return sensorDto is null ? new Result<Domain.Models.Sensor>(new SensorNotFoundException(sensorId)) : (Result<Domain.Models.Sensor>)sensorDto.ToSensor();
        }
        catch (Exception ex)
        {
            return new Result<Domain.Models.Sensor>(ex);
        }
    }

    public async Task<Result<SensorHistory>> CreateSensorHistoryAsync(SensorHistory sensorHistory)
    {
        try
        {
            EntityEntry<SensorHistoryDto> result = await _dbContext.SensorHistories.AddAsync(sensorHistory.ToSensorHistoryDto()).ConfigureAwait(false);
            _ = _dbContext.SaveChanges();
            return result.Entity.ToSensorHistory();
        }
        catch (Exception ex)
        {
            return new Result<SensorHistory>(ex);
        }
    }

    public async Task<Result<Domain.Models.Sensor>> UpdateSensorAsync(Domain.Models.Sensor sensor)
    {
        try
        {
            _ = _dbContext.Sensors.Update(sensor.ToSensorDto());
            _ = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return sensor;
        }
        catch (Exception ex)
        {
            return new Result<Domain.Models.Sensor>(ex);
        }
    }

    public Task<Result<IEnumerable<SensorHistory>>> GetSensorHistoriesAsync(int? sensorId, int limit)
    {
        IEnumerable<SensorHistoryDto> sensorHistories = _dbContext.SensorHistories;
        if (sensorId.HasValue)
        {
            sensorHistories = sensorHistories.Where(s => s.SensorId == sensorId.Value);
        }
        IEnumerable<SensorHistory?> sensorHitories = sensorHistories.OrderByDescending(s => s.GetTemperatureDate).Take(limit).Select(x => x.ToSensorHistory());
        return Task.FromResult(new Result<IEnumerable<SensorHistory>>(sensorHitories));
    }

    public async Task<Result<IEnumerable<Domain.Models.Sensor>>> GetSensorsAync()
    {
        try
        {
            IEnumerable<SensorDto> sensorDtos = _dbContext.Sensors;
            return await Task.FromResult(new Result<IEnumerable<Domain.Models.Sensor>>(sensorDtos.Select(s => s.ToSensor())));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(new Result<IEnumerable<Domain.Models.Sensor>>(ex));
        }
    }
}