namespace MetricsDelta
{
    public interface IMetricsReportStripper
    {
        void Strip(string inputFilePath, string outputFilePath);
    }
}