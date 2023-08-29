﻿// See https://aka.ms/new-console-template for more information
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

using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddConsole()
    );

var logger = loggerFactory.CreateLogger<Program>();

var hostBuilder = new HostBuilder();

var host = hostBuilder.ConfigureAppConfiguration((hostingContext, cfgBuilder) =>
{
    var cfg = cfgBuilder.AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("default_grading_thresholds.json", optional: true)
    .AddEnvironmentVariables().Build();

    hostBuilder.ConfigureServices(sc =>
    {
        {
            sc.AddSingleton<ILogger>(logger);
            sc.Configure<GradingThresholds>(cfg.GetSection("GradingThresholds"));
       }
    })
    .SetupCommandLine(args)
    .SetupMetricsReportGrader()
    .SetupGradeProvider()
    .SetupXmlReportWriter()
    .SetupMetricsReportStripper();
}).Build();

var runSettings = host.Services.GetRequiredService<IOptions<MetricDeltaCfg>>();

if (!XmlHelper.TryRestoreFromXml(
    runSettings.Value.PreviousMetricsFilePath,
    out XmlCodeMetricsReport? previousModel,
    out string? errorMessage) || previousModel is null)
{
    logger.LogError($"Unable to restore CodeMetricsReport given at path '{runSettings.Value.PreviousMetricsFilePath}'. Reason: {errorMessage}");
    return -1;
}

if (!XmlHelper.TryRestoreFromXml(
    runSettings.Value.CurrentMetricsFilePath,
    out XmlCodeMetricsReport? currentModel,
    out errorMessage) || currentModel is null)
{
    logger.LogError($"Unable to restore CodeMetricsReport given at path '{runSettings.Value.CurrentMetricsFilePath}'. Reason: {errorMessage}");
    return -1;
}

var outputReportFilePath = runSettings.Value.ReportFilePath;

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




