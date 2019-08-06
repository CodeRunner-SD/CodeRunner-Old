using System;

namespace CodeRunner.Executors
{
    public class ExecutorResult
    {
        public string Output { get; set; } = "";

        public string Error { get; set; } = "";

        public long MaximumMemory { get; set; }

        public TimeSpan RunningTime => EndTime - StartTime;

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public int ExitCode { get; set; }

        public ExecutorState State { get; set; }
    }
}
