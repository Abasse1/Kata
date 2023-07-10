using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Serenza.Sensor.Infrastructure.Dto;

public class SensorHistoryDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SensorHistoryId { get; init; }

    public double Temperature { get; init; }

    public DateTimeOffset GetTemperatureDate { get; init; }

    public int SensorId { get; init; }

    public SensorDto? Sensor { get; init; }
}