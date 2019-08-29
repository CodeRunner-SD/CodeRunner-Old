using System;
using System.Text;

namespace CodeRunner.Executors
{
    public class ExecutorResult
    {
        private readonly StringBuilder OutputBuilder = new StringBuilder();
        private readonly StringBuilder ErrorBuilder = new StringBuilder();
        private string? output, error;

        public void AppendOutput(string content) => _ = OutputBuilder.Append(content);

        public void AppendError(string content) => _ = ErrorBuilder.Append(content);

        public string Output
        {
            get
            {
                if (output == null)
                {
                    output = OutputBuilder.ToString();
                }

                return output;
            }
        }

        public string Error
        {
            get
            {
                if (error == null)
                {
                    error = ErrorBuilder.ToString();
                }

                return error;
            }
        }

        public long MaximumMemory { get; set; }

        public TimeSpan RunningTime => EndTime - StartTime;

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public int ExitCode { get; set; }

        public ExecutorState State { get; set; }
    }
}
