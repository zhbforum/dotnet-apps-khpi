using IoTTelemetryLab.Models;

namespace IoTTelemetryLab.Processing;

/// <summary>
/// Defines the possible status levels of a reading.
/// </summary>
public enum SafetyStatus
{
    Normal,
    Warning,
    Critical
}

/// <summary>
/// Determines the safety status of a sensor reading.
/// </summary>
public static class StatusEvaluator
{
    /// <summary>
    /// Evaluates the status of a reading based on the sensor type.
    /// </summary>
    /// <param name="type">Sensor type.</param>
    /// <param name="value">Reading value.</param>
    /// <returns>The status level of the reading.</returns>
    public static SafetyStatus Evaluate(
        SensorType type,
        double value) => type switch
        {
            SensorType.Temperature => value switch
            {
                < 0 or > 80 => SafetyStatus.Critical,
                >= 60 and <= 80 => SafetyStatus.Warning,
                _ => SafetyStatus.Normal
            },

            SensorType.Vibration => value switch
            {
                > 5.0 => SafetyStatus.Critical,
                > 2.5 => SafetyStatus.Warning,
                _ => SafetyStatus.Normal
            },

            SensorType.Voltage => value switch
            {
                < 200 or > 240 => SafetyStatus.Critical,
                < 210 or > 230 => SafetyStatus.Warning,
                _ => SafetyStatus.Normal
            },

            // Variant 5: noise above 100 dB is Critical, from 85 to 100 dB is Warning.
            SensorType.SoundLevel => value switch
            {
                > 100 => SafetyStatus.Critical,
                > 85 and <= 100 => SafetyStatus.Warning,
                _ => SafetyStatus.Normal
            },

            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
}
