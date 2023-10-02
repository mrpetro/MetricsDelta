using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsDelta
{
    public interface IReportVisitorFactory
    {
        TVisitor Create<TVisitor>() where TVisitor : IReportVisitor;
    }
}
