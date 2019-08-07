using CodeRunner.Loggings;

namespace CodeRunner.Pipelines
{
    public class OperationContext
    {
        public OperationContext(ServiceScope services, LogScope logs)
        {
            Services = services;
            Logs = logs;
        }

        public ServiceScope Services { get; }

        public LogScope Logs { get; }

        public bool IsEnd { get; set; } = false;

        public bool IgnoreResult { get; set; } = false;
    }

    public class OperationContext<TOrigin, TResult> : OperationContext
    {
        public OperationContext(ServiceScope services, TOrigin origin, TResult result, LogScope logs) : base(services, logs)
        {
            Origin = origin;
            Result = result;
        }

        public TOrigin Origin { get; }

        public TResult Result { get; }
    }
}
