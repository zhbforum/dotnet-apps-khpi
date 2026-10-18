using IoTTelemetryLab.Models;

namespace IoTTelemetryLab.Monitoring.Subscribers;

public class CriticalByHourSubscriber
{
    private readonly Dictionary<int, List<TelemetryReading>>
        _criticalByHour = new();

    public IReadOnlyDictionary<int, List<TelemetryReading>>
        CriticalByHour => _criticalByHour;

    public void OnStatusEvaluated(
        TelemetryReading reading,
        SafetyStatus status)
    {
        if (status != SafetyStatus.Critical)
        {
            return;
        }

        int hour = reading.Timestamp.Hour;

        if (!_criticalByHour.TryGetValue(hour, out var readings))
        {
            readings = new List<TelemetryReading>();
            _criticalByHour[hour] = readings;
        }

        readings.Add(reading);
    }

    public void PrintSummary()
    {
        Console.WriteLine("\nCritical readings grouped by hour:");

        if (_criticalByHour.Count == 0)
        {
            Console.WriteLine("No Critical readings found");
            return;
        }

        foreach (var group in _criticalByHour.OrderBy(x => x.Key))
        {
            Console.WriteLine(
                $"{group.Key:00}:00-{group.Key:00}:59 -> " +
                $"{group.Value.Count}");
        }
    }
}
