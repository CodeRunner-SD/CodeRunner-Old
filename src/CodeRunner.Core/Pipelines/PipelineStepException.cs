using System;

namespace CodeRunner.Pipelines
{
    public class PipelineStepException : Exception
    {
        public PipelineStepException(string message) : base(message)
        {
        }

        public PipelineStepException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PipelineStepException()
        {
        }
    }
}
