namespace MetricsDelta.Model
{
    public interface IMetric
    {
        string? Name { get; }

        int Value { get; }
    }
}