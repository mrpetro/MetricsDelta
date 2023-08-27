using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MetricsDelta.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReportGrader(this IServiceCollection services)
        {
            services.TryAddSingleton<IReportVisitor, ReportGrader>();
            return services;
        }

        public static IServiceCollection AddMetricsReportStripper(this IServiceCollection services)
        {
            services.AddSingleton<IMetricsReportStripper, MetricsReportStripper>();
            return services;
        }

        public static IServiceCollection AddGradeProvider(this IServiceCollection services)
        {
            services.TryAddSingleton<IGradeProvider, GradeProvider>();
            return services;
        }

        public static IServiceCollection AddXmlReportWriter(this IServiceCollection services)
        {
            services.AddSingleton<IReportWriter, XmlReportWriter>((sp) =>
            {
                var sts = new XmlWriterSettings()
                {
                    Indent = true,
                };

                var options = sp.GetRequiredService<IOptions<RunSettings>>();

                return new XmlReportWriter(XmlWriter.Create(options.Value.ReportFilePath, sts));
            });
            return services;
        }
    }
}
