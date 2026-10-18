using IoTTelemetryLab.Models;

namespace IoTTelemetryLab.Evaluation;

public class SensorEvaluatorRegistry
{
    private readonly Dictionary<SensorType, ISensorEvaluator> _evaluators = new();

    public void Register(
        SensorType type,
        ISensorEvaluator evaluator)
    {
        _evaluators[type] = evaluator;
    }

    public void Register(
        SensorType type,
        Func<double, SafetyStatus> evaluatorFunc)
    {
        _evaluators[type] = new DelegateSensorEvaluator(evaluatorFunc);
    }

    public SafetyStatus Evaluate(
        SensorType type,
        double value)
    {
        if (!_evaluators.TryGetValue(type, out var evaluator))
        {
            throw new InvalidOperationException(
                $"No registered evaluator for the type {type}");
        }

        return evaluator.Evaluate(value);
    }

    private sealed class DelegateSensorEvaluator : ISensorEvaluator
    {
        private readonly Func<double, SafetyStatus> _evaluatorFunc;

        public DelegateSensorEvaluator(
            Func<double, SafetyStatus> evaluatorFunc)
        {
            _evaluatorFunc = evaluatorFunc;
        }

        public SafetyStatus Evaluate(double value)
        {
            return _evaluatorFunc(value);
        }
    }
}
