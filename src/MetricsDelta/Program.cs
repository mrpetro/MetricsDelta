// See https://aka.ms/new-console-template for more information
using CSTest;
using MetricsDelta;
using MetricsDelta.Configuration;
using MetricsDelta.Extensions;
using MetricsDelta.Helpers;
using MetricsDelta.Model.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Xml;

var rootClSettings = CLHelper.SetupCommandLine(args);

using var loggerFactory = LoggerFactory.Create(builder => {
    builder.ClearProviders();
    builder.AddConsole();
});

var logger = loggerFactory.CreateLogger<Program>();

var hostBuilder = new HostBuilder()
    .SetupLogger(logger)
    .SetupMetricsReportGrader()
    .SetupGradeProvider()
    .SetupXmlReportWriter(rootClSettings.ReportFilePath)
    .SetupMetricsReportStripper();

var host = hostBuilder.ConfigureAppConfiguration((hostingContext, cfgBuilder) =>
{
    cfgBuilder.AddJsonFile("appsettings.json", optional: true);

    if(File.Exists(rootClSettings.SettingsFilePath))
        cfgBuilder.AddJsonFile(rootClSettings.SettingsFilePath, optional: true);

    var cfgRoot = cfgBuilder.AddEnvironmentVariables()
    .Build();

    hostBuilder.ConfigureServices(sc =>
    {
        sc.Configure<GradingThresholds>(cfgRoot.GetSection("GradingThresholds"));
    });
}).Build();

if (!XmlHelper.TryRestoreFromXml(
    rootClSettings.PreviousMetricsFilePath,
    out XmlCodeMetricsReport? previousModel,
    out string? errorMessage) || previousModel is null)
{
    logger.LogError($"Unable to restore CodeMetricsReport given at path '{rootClSettings.PreviousMetricsFilePath}'. Reason: {errorMessage}");
    return -1;
}

if (!XmlHelper.TryRestoreFromXml(
    rootClSettings.CurrentMetricsFilePath,
    out XmlCodeMetricsReport? currentModel,
    out errorMessage) || currentModel is null)
{
    logger.LogError($"Unable to restore CodeMetricsReport given at path '{rootClSettings.CurrentMetricsFilePath}'. Reason: {errorMessage}");
    return -1;
}

var outputReportFilePath = rootClSettings.ReportFilePath;

using var tokenSource = new CancellationTokenSource();
Console.CancelKeyPress += delegate
{
    tokenSource.Cancel();
};

try
{
    var reportVisitor = host.Services.GetRequiredService<IReportVisitor>();
    var deltaComparer = new ReportComparer(reportVisitor);
    deltaComparer.CompareAsync(previousModel, currentModel, tokenSource.Token).GetAwaiter().GetResult();

    var result = 0;

    if (reportVisitor.AnyDeltaDegradations)
    {
        logger.LogError("Metric delta degradations detected.");
        result |= 1;
    }

    if (reportVisitor.AnyBadMetricGrades)
    {
        logger.LogError("Bad metric grades detected.");
        result |= 2;
    }

    return result;
}
catch (OperationCanceledException)
{
    logger.LogError("Operation Cancelled.");
    return -1;
}
catch (Exception ex)
{
    logger.LogCritical($"Exception occurred:{Environment.NewLine}{ex}");
    return -2;
}
finally
{
    //NOTE: This is here due to some problems with ConsoleLoggerProcessor.
    //It minimizes issue with missing final messages
    Thread.Sleep(1500);
}




