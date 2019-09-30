using CodeRunner.Diagnostics;
using CodeRunner.Loggings;
using System.Diagnostics.CodeAnalysis;

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

        public bool IsStopped { get; set; }
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

        [DoesNotReturn]
        public TResult IgnoreResult()
        {
            Logs.Log("Pipeline ignore result.", LogLevel.Debug);
            throw new PipelineResultIgnoreException();
        }
    }
}
