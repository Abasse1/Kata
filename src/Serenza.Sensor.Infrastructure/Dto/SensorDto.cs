using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Serenza.Sensor.Infrastructure.Dto;
public record SensorDto
{
    public SensorDto()
    {
        SensorHistories = new List<SensorHistoryDto>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SensorId { get; init; }

    public string State { get; init; } = string.Empty;

    public double TemperatureMax { get; init; }

    public double TemperatureMin { get; init; }

    public IList<SensorHistoryDto> SensorHistories { get; init; }
}