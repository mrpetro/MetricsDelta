using MetricsDelta.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Xml;

namespace MetricsDelta.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

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

        public static IServiceCollection AddXmlReportWriter(this IServiceCollection services, string reportFilePath)
        {
            services.AddSingleton<IReportWriter, XmlReportWriter>((sp) =>
            {
                var sts = new XmlWriterSettings()
                {
                    Indent = true,
                };

                return new XmlReportWriter(XmlWriter.Create(reportFilePath, sts));
            });
            return services;
        }

        #endregion
    }
}