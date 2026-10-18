using IoTTelemetryLab.Models;

namespace IoTTelemetryLab.Data;

/// <summary>
/// Generates test telemetry data.
/// </summary>
public static class TelemetryGenerator
{
    /// <summary>
    /// Generates a batch of raw sensor readings,
    /// including simulated hardware errors.
    /// </summary>
    /// <param name="deviceId">Sensor identifier.</param>
    /// <param name="count">Number of readings.</param>
    /// <param name="minValue">Minimum value.</param>
    /// <param name="maxValue">Maximum value.</param>
    /// <returns>A list of generated readings.</returns>
    public static List<TelemetryReading> GenerateRawBatch(
        string deviceId,
        int count,
        double minValue,
        double maxValue)
    {
        var random = new Random();
        var readings = new List<TelemetryReading>();
        var baseTime = DateTime.UtcNow;

        for (int i = 0; i < count; i++)
        {
            double value = random.NextDouble() < 0.1
                ? (random.Next(2) == 0 ? -999.0 : double.NaN)
                : Math.Round(
                    minValue + random.NextDouble() * (maxValue - minValue),
                    2);

            readings.Add(new TelemetryReading(
                DeviceId: deviceId,
                Value: value,
                Timestamp: baseTime.AddSeconds(i * 5)));
        }

        return readings;
    }
}
