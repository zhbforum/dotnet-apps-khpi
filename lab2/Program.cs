using IoTTelemetryLab.Data;
using IoTTelemetryLab.Evaluation;
using IoTTelemetryLab.Models;
using IoTTelemetryLab.Monitoring;
using IoTTelemetryLab.Monitoring.Subscribers;
using IoTTelemetryLab.Processing;

const string deviceId = "rotation-sensor-05";
const SensorType sensorType = SensorType.RotationSpeed;

var registry = EvaluatorSetup.CreateDefaultRegistry();

// Task A: register RotationSpeed using a Strategy class.
registry.Register(
    SensorType.RotationSpeed,
    new RotationSpeedEvaluator());

double[] testValues =
{
    400,
    650,
    1000,
    2600,
    3200
};

var strategyResults = testValues
    .Select(value =>
        registry.Evaluate(
            SensorType.RotationSpeed,
            value))
    .ToArray();

// Task B: replace the same evaluator with Func<double, SafetyStatus>.
registry.Register(
    SensorType.RotationSpeed,
    value => value switch
    {
        < 500 or > 3000 => SafetyStatus.Critical,
        < 800 or > 2500 => SafetyStatus.Warning,
        _ => SafetyStatus.Normal
    });

var funcResults = testValues
    .Select(value =>
        registry.Evaluate(
            SensorType.RotationSpeed,
            value))
    .ToArray();

Console.WriteLine("Strategy and Func comparison:");

for (int i = 0; i < testValues.Length; i++)
{
    Console.WriteLine(
        $"{testValues[i],7} RPM -> " +
        $"Strategy: {strategyResults[i],-8} | " +
        $"Func: {funcResults[i],-8}");
}

Console.WriteLine(
    $"\nResults are equal: " +
    $"{strategyResults.SequenceEqual(funcResults)}");

var monitor = new TelemetryMonitor(
    registry,
    sensorType);

var counter = new CriticalCounterSubscriber();
var criticalByHour = new CriticalByHourSubscriber();

// Multicast event: several independent subscribers.
monitor.OnStatusEvaluated +=
    ConsoleAlertSubscriber.OnStatusEvaluated;

monitor.OnStatusEvaluated +=
    counter.OnStatusEvaluated;

monitor.OnStatusEvaluated +=
    criticalByHour.OnStatusEvaluated;

var rawData = TelemetryGenerator.GenerateRawBatch(
    deviceId,
    count: 50,
    minValue: 300,
    maxValue: 3300);

Console.WriteLine(
    $"\nReceived readings: {rawData.Count}");

var cleaned = TelemetryCleaner
    .RemoveInvalid(rawData)
    .ToList();

Console.WriteLine(
    $"Readings after cleaning: {cleaned.Count}");

foreach (var reading in cleaned)
{
    monitor.Process(reading);
}

Console.WriteLine(
    $"\nCritical readings counted by subscriber: " +
    $"{counter.CriticalCount}");

var analyticsCritical = TelemetryAnalytics.CountByStatus(
    cleaned,
    sensorType,
    SafetyStatus.Critical,
    registry);

Console.WriteLine(
    $"Critical readings counted by analytics: " +
    $"{analyticsCritical}");

// Variant 5: group Critical readings by hour.
criticalByHour.PrintSummary();
