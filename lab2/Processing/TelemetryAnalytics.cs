using IoTTelemetryLab.Evaluation;
using IoTTelemetryLab.Models;

namespace IoTTelemetryLab.Processing;

/// <summary>
/// Contains methods for analyzing telemetry data.
/// </summary>
public static class TelemetryAnalytics
{
    public static int CountByStatus(
        IEnumerable<TelemetryReading> readings,
        SensorType type,
        SafetyStatus target,
        SensorEvaluatorRegistry registry)
    {
        return readings.Count(
            r => registry.Evaluate(type, r.Value) == target);
    }

    public static IEnumerable<double> MovingAverage(
        IEnumerable<TelemetryReading> readings,
        int windowSize)
    {
        var values = readings
            .Select(r => r.Value)
            .ToList();

        for (int i = 0; i <= values.Count - windowSize; i++)
        {
            yield return values
                .Skip(i)
                .Take(windowSize)
                .Average();
        }
    }
}
