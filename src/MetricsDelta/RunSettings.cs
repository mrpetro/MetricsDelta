using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsDelta
{
    public class RunSettings
    {
        public string PreviousMetricsFilePath { get; set; }
        public string CurrentMetricsFilePath { get; set; }
        public string ReportFilePath { get; set; }
        public bool FailOnDeltaDegradation { get; set; }
        public bool FailOnFatalMetricGrade { get; set; }
    }
}
