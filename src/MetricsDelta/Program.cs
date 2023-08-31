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

var hostBuilder = new HostBuilder()
    .SetupMetricsReportGrader()
    .SetupGradeProvider()
    .SetupDeltaSeverityProvider()
    .SetupXmlReportWriter(rootClSettings.ReportFilePath)
    .SetupMetricsReportStripper()
    .ConfigureLogging((hostingContext, loggingBuilder) =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
        loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
    });

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

var logger = host.Services.GetRequiredService<ILogger<Program>>();

try
{
    if (!rootClSettings.ValidateSettings(logger))
        return -1;

    XmlCodeMetricsReport? previousModel = null;
    XmlCodeMetricsReport? currentModel = null;

    if (rootClSettings.PreviousMetricsFilePath is not null && !XmlHelper.TryRestoreFromXml(
        rootClSettings.PreviousMetricsFilePath,
        out previousModel,
        out string? errorMessage))
    {
        logger.LogError($"Unable to restore CodeMetricsReport given at path '{rootClSettings.PreviousMetricsFilePath}'. Reason: {errorMessage}");
        return -1;
    }

    if (rootClSettings.CurrentMetricsFilePath is not null && !XmlHelper.TryRestoreFromXml(
        rootClSettings.CurrentMetricsFilePath,
        out currentModel,
        out errorMessage))
    {
        logger.LogError($"Unable to restore CodeMetricsReport given at path '{rootClSettings.CurrentMetricsFilePath}'. Reason: {errorMessage}");
        return -1;
    }

    if (previousModel is null)
        throw new InvalidOperationException($"{nameof(previousModel)} is not expected to be null here.");

    if (currentModel is null)
        throw new InvalidOperationException($"{nameof(currentModel)} is not expected to be null here.");

    var outputReportFilePath = rootClSettings.ReportFilePath;

    using var tokenSource = new CancellationTokenSource();
    Console.CancelKeyPress += delegate
    {
        tokenSource.Cancel();
    };

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




