using CodeRunner.Diagnostics;
using CodeRunner.Loggings;
using System.Collections.Generic;

namespace CodeRunner.Pipelines
{
    public class PipelineResult<T> where T : class
    {
        public bool IsOk => Exception == null;

        public bool IsError => !IsOk;

        public T? Result { get; }

        public PipelineStepException? Exception { get; }

        public IReadOnlyList<LogItem> Logs { get; }

        public PipelineResult(T? result, PipelineStepException? ex, IReadOnlyList<LogItem> logs)
        {
            Assert.IsNotNull(logs);
            Assert.IsTrue(Exception == null && result != null || Exception != null);

            Exception = ex;
            Result = result;
            Logs = logs;
        }
    }
}
