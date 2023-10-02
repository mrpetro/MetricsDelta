namespace MetricsDelta
{
    public interface IDeltaVisitor
    {
        #region Public Methods

        bool AnyDeltaDegradations { get; }

        bool AnyBadMetricGrades { get; }

        void VisitMetric(DeltaState deltaState, string metricName, int currentValue, int delta);

        void BeginVisitTarget(string targetName, DeltaState deltaState);

        void EndVisitTarget(string targetName, DeltaState deltaState);

        void BeginVisitAssembly(string assemblyName, DeltaState deltaState);

        void EndVisitAssembly(string assemblyName, DeltaState deltaState);

        void BeginVisitReport();

        void EndVisitReport();

        #endregion Public Methods
    }
}