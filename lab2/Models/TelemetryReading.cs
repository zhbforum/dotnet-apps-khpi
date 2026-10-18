namespace IoTTelemetryLab.Models;

/// <summary>
/// Represents a single IoT sensor reading.
/// </summary>
public readonly record struct TelemetryReading(
    string DeviceId,
    double Value,
    DateTime Timestamp);
