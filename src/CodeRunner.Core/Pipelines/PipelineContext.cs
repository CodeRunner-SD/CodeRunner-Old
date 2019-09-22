using CodeRunner.Diagnostics;
using CodeRunner.Loggings;

namespace CodeRunner.Pipelines
{
    public class PipelineContext
    {
        public PipelineContext(ServiceScope services, LoggerScope logs)
        {
            Assert.IsNotNull(services);
            Assert.IsNotNull(logs);

            Services = services;
            Logs = logs;
        }

        public ServiceScope Services { get; }

        public LoggerScope Logs { get; }

        public bool IsEnd { get; set; } = false;

        public bool IgnoreResult { get; set; } = false;
    }

    public class PipelineContext<TOrigin, TResult> : PipelineContext where TResult : class
    {
        public PipelineContext(ServiceScope services, TOrigin origin, TResult? result, LoggerScope logs) : base(services, logs)
        {
            Assert.IsNotNull(services);
            Assert.IsNotNull(origin);
            Assert.IsNotNull(logs);

            Origin = origin;
            Result = result;
        }

        public TOrigin Origin { get; }

        public TResult? Result { get; }
    }
}
