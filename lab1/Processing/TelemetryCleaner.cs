using IoTTelemetryLab.Models;

namespace IoTTelemetryLab.Processing;

/// <summary>
/// Performs telemetry data cleaning.
/// </summary>
public static class TelemetryCleaner
{
    private const double ErrorMarker = -999.0;

    /// <summary>
    /// Removes invalid readings from the dataset.
    /// </summary>
    /// <param name="raw">Raw readings.</param>
    /// <returns>A sequence of valid readings.</returns>
    public static IEnumerable<TelemetryReading> RemoveInvalid(
        IEnumerable<TelemetryReading> raw)
    {
        return raw.Where(r =>
            !double.IsNaN(r.Value) &&
            r.Value != ErrorMarker);
    }
}
