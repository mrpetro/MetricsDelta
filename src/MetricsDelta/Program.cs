// See https://aka.ms/new-console-template for more information
using MetricsDelta;
using MetricsDelta.Commands;
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

var rootCommand = new RootCommand();
rootCommand.AddCommand(ReportGradeCommand.Create(args));
rootCommand.AddCommand(ReportCompareCommand.Create(args));

await rootCommand.InvokeAsync(args);








