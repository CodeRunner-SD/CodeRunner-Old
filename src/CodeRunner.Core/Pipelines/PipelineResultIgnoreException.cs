using System;

namespace CodeRunner.Pipelines
{
    public class PipelineResultIgnoreException : PipelineFlowException
    {
        public PipelineResultIgnoreException(string message) : base(message)
        {
        }

        public PipelineResultIgnoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PipelineResultIgnoreException()
        {
        }
    }
}
