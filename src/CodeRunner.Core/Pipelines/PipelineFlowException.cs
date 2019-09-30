using System;

namespace CodeRunner.Pipelines
{
    public abstract class PipelineFlowException : Exception
    {
        public PipelineFlowException(string message) : base(message)
        {
        }

        public PipelineFlowException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PipelineFlowException()
        {
        }
    }
}
