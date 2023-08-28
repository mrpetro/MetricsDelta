using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsDelta
{
    public enum DeltaState
    {
        Existing,
        New,
        Removed
    }

    public enum DeltaSeverity
    {
        Irrelevant,
        Improved,
        Declined
    }

    public enum MetricGrade
    {
        Good,
        Poor,
        Bad
    }

    public interface IReportWriter
    {
        void WriteMetric(string name, int value, int delta, MetricGrade valueGrade, DeltaState deltaState, DeltaSeverity deltaSeverity);
        void BeginWriteTarget(string name, DeltaState deltaState);
        void EndWriteTarget(string name, DeltaState deltaState);
        void BeginWriteAssembly(string name, DeltaState deltaState);
        void EndWriteAssembly(string name, DeltaState deltaState);
        void BeginWriteReport();
        void EndWriteReport();
    }
}
