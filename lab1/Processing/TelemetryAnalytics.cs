using IoTTelemetryLab.Models;

namespace IoTTelemetryLab.Processing;

/// <summary>
/// Contains methods for analyzing telemetry data.
/// </summary>
public static class TelemetryAnalytics
{
    /// <summary>
    /// Finds the timestamp of the first Critical reading.
    /// Uses the LINQ methods Where, Select, and FirstOrDefault.
    /// </summary>
    /// <param name="readings">Cleaned readings.</param>
    /// <param name="type">Sensor type.</param>
    /// <returns>
    /// The timestamp of the first Critical reading, or null
    /// if no such reading exists.
    /// </returns>
    public static DateTime? FirstCriticalTimestamp(
        IEnumerable<TelemetryReading> readings,
        SensorType type)
    {
        return readings
            .Where(r =>
                StatusEvaluator.Evaluate(type, r.Value)
                == SafetyStatus.Critical)
            .Select(r => (DateTime?)r.Timestamp)
            .FirstOrDefault();
    }
}
