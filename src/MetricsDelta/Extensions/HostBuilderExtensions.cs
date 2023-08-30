using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using MetricsDelta.Configuration;
using Microsoft.Extensions.Logging;

namespace MetricsDelta.Extensions
{
    /// <summary>
    /// Various extension methods for IHostBuilder.
    /// </summary>
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static IHostBuilder SetupLogger(this IHostBuilder hostBuilder, ILogger logger)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddSingleton<ILogger>(logger));
        }

        public static IHostBuilder SetupMetricsReportGrader(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddReportGrader());
        }

        public static IHostBuilder SetupMetricsReportStripper(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddMetricsReportStripper());
        }

        public static IHostBuilder SetupGradeProvider(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddGradeProvider());
        }
        public static IHostBuilder SetupXmlReportWriter(this IHostBuilder hostBuilder, string reportFilePath)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddXmlReportWriter(reportFilePath));
        }

        #endregion Public Methods

    }
}
