using Microsoft.Extensions.Caching.Memory;
using Serenza.Sensor.Infrastructure.Dto;

namespace Serenza.Sensor.Infrastructure.Extensions;

internal static class SensorMemoryCacheExtension
{
    internal static void GetSetSensorsCache(this IMemoryCache memoryCache, IEnumerable<SensorDto> sensorDtos)
    {
        _ = new MemoryCacheEntryOptions()
                  .SetPriority(CacheItemPriority.Normal)
                  .SetSize(5);
        _ = memoryCache.Set(Constants.SensorMemoryCacheKey, sensorDtos);
    }
}