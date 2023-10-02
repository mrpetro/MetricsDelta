using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Xml;

namespace MetricsDelta.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        public static IServiceCollection AddReportWalker(this IServiceCollection services)
        {
            services.TryAddSingleton<IReportWalker, ReportWalker>();
            return services;
        }

        public static IServiceCollection AddReportGraderFactory(this IServiceCollection services)
        {
            services.TryAddSingleton<IReportVisitorFactory, ReportVisitorFactory>();
            return services;
        }

        public static IServiceCollection AddDeltaGrader(this IServiceCollection services)
        {
            services.TryAddSingleton<IDeltaVisitor, DeltaGrader>();
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

        public static IServiceCollection AddDeltaSeverityProvider(this IServiceCollection services)
        {
            services.TryAddSingleton<IDeltaSeverityProvider, DeltaSeverityProvider>();
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