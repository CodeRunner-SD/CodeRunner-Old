using CodeRunner.Loggers;

namespace CodeRunner.Pipelines
{
    public class ExecutionContext<TOrigin, TResult>
    {
        public ExecutionContext(ServiceScope services, TOrigin origin, TResult result, LogScope logs)
        {
            Services = services;
            Origin = origin;
            Result = result;
            Logs = logs;
        }

        public ServiceScope Services { get; }

        public TOrigin Origin { get; }

        public TResult Result { get; }

        public LogScope Logs { get; }
    }
}
