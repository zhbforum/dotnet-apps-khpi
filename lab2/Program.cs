using IoTTelemetryLab.Data;
using IoTTelemetryLab.Models;
using IoTTelemetryLab.Processing;

const string deviceId = "sound-sensor-05";
const SensorType sensorType = SensorType.SoundLevel;

var rawData = TelemetryGenerator.GenerateRawBatch(
    deviceId,
    count: 50,
    minValue: 60,
    maxValue: 130);

Console.WriteLine($"Отримано показів: {rawData.Count}");

var cleaned = TelemetryCleaner
    .RemoveInvalid(rawData)
    .ToList();

Console.WriteLine($"Після очищення: {cleaned.Count}");

var firstCriticalTimestamp =
    TelemetryAnalytics.FirstCriticalTimestamp(cleaned, sensorType);

if (firstCriticalTimestamp.HasValue)
{
    Console.WriteLine(
        $"Час першого Critical-показу: {firstCriticalTimestamp.Value:O}");
}
else
{
    Console.WriteLine("Critical-показів не знайдено.");
}
