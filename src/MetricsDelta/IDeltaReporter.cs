using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsDelta
{
    public interface IDeltaReporter
    {
        void Verbose(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
