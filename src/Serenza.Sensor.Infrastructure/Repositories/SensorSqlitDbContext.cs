using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serenza.Sensor.Domain.Enum;
using Serenza.Sensor.Infrastructure.Dto;

namespace Serenza.Sensor.Infrastructure.Repositories;

public class SensorSqlitDbContext : DbContext
{
    private readonly IEnumerable<SensorDto> _initialSenorList;

    public SensorSqlitDbContext(IOptionsMonitor<List<SensorDto>> options)
    {
        _initialSenorList = options.CurrentValue;
    }

    public SensorSqlitDbContext(DbContextOptions<SensorSqlitDbContext> options, IOptionsMonitor<List<SensorDto>> optionsMonitor) : base(options)
    {
        _initialSenorList = optionsMonitor.CurrentValue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        IEnumerable<SensorDto> updatedSensors = _initialSenorList.Select(sensor =>
        {
            return sensor.State == SensorState.HOT.ToString()
                ? (sensor with { TemperatureMax = short.MaxValue })
                : sensor.State == SensorState.COLD.ToString() ? (sensor with { TemperatureMin = short.MinValue }) : sensor;
        });
        _ = modelBuilder.Entity<SensorDto>().HasData(updatedSensors);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<SensorDto> Sensors { get; set; }

    public DbSet<SensorHistoryDto> SensorHistories { get; set; }
}