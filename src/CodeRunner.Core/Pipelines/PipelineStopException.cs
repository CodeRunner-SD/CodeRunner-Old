using System;

namespace CodeRunner.Pipelines
{
    public class PipelineStopException : PipelineFlowException
    {
        public PipelineStopException(string message) : base(message)
        {
        }

        public PipelineStopException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PipelineStopException()
        {
        }
    }
}
