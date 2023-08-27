using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MetricsDelta.Model
{
    public interface ICodeMetricsReport
    {
        string? Version { get; }

        IEnumerable<ITarget> Targets { get; }
    }
}
