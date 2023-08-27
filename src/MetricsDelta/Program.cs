// See https://aka.ms/new-console-template for more information
using CSTest;
using MetricsDelta;
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



var hostBuilder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: true);



    config.AddEnvironmentVariables();

});

using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddConsole()
    );


var logger = loggerFactory.CreateLogger<Program>();
hostBuilder.ConfigureServices(ser => ser.AddSingleton<ILogger>(logger));

hostBuilder.SetupCommandLine(args);
hostBuilder.SetupMetricsReportGrader();
hostBuilder.SetupGradeProvider();
hostBuilder.SetupXmlReportWriter();
hostBuilder.SetupMetricsReportStripper();

var host = hostBuilder.Build();

var runSettings = host.Services.GetRequiredService<IOptions<RunSettings>>();
var reportVisitor = host.Services.GetRequiredService<IReportVisitor>();

if (!XmlHelper.TryRestoreFromXml(
    runSettings.Value.PreviousMetricsFilePath,
    out XmlCodeMetricsReport? previousModel,
    out string? errorMessage) || previousModel is null)
{
    Console.WriteLine($"Unable to restore CodeMetricsReport given at path '{runSettings.Value.PreviousMetricsFilePath}'. Reason: {errorMessage}");
    return -1;
}

if (!XmlHelper.TryRestoreFromXml(
    runSettings.Value.CurrentMetricsFilePath,
    out XmlCodeMetricsReport? currentModel,
    out errorMessage) || currentModel is null)
{
    Console.WriteLine($"Unable to restore CodeMetricsReport given at path '{runSettings.Value.CurrentMetricsFilePath}'. Reason: {errorMessage}");
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
    var deltaComparer = new ReportComparer(reportVisitor);
    deltaComparer.CompareAsync(previousModel, currentModel, tokenSource.Token).GetAwaiter().GetResult();


    if (reportVisitor.AnyDeltaDegradations)
    {
        Console.WriteLine("Metric delta degradations detected.");
    }

    if (reportVisitor.AnyFatalMetricGrades)
    {
        Console.WriteLine("Fatal metric grades detected.");
    }

    if(reportVisitor.AnyDeltaDegradations && runSettings.Value.FailOnDeltaDegradation)
            return 1;

    if (reportVisitor.AnyFatalMetricGrades && runSettings.Value.FailOnFatalMetricGrade)
            return 1;

    return 0;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation Cancelled.");
    return -1;
}




