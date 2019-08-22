using CodeRunner.Loggings;

namespace CodeRunner.Pipelines
{
    public class PipelineContext
    {
        public PipelineContext(ServiceScope services, LoggerScope logs)
        {
            Services = services;
            Logs = logs;
        }

        public ServiceScope Services { get; }

        public LoggerScope Logs { get; }

        public bool IsEnd { get; set; } = false;

        public bool IgnoreResult { get; set; } = false;
    }

    public class PipelineContext<TOrigin, TResult> : PipelineContext
    {
        public PipelineContext(ServiceScope services, TOrigin origin, TResult result, LoggerScope logs) : base(services, logs)
        {
            Origin = origin;
            Result = result;
        }

        public TOrigin Origin { get; }

        public TResult Result { get; }
    }
}
