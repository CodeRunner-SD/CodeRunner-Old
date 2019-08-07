using CodeRunner.Loggings;
using System;

namespace CodeRunner.Pipelines
{
    public class PipelineResult<T>
    {
        public bool IsOk => Exception == null;

        public bool IsError => !IsOk;

        public T Result { get; }

        public Exception? Exception { get; }

        public LogItem[] Logs { get; }

        public PipelineResult(T result, Exception? ex, LogItem[] logs)
        {
            Exception = ex;
            Result = result;
            Logs = logs;
        }
    }
}
