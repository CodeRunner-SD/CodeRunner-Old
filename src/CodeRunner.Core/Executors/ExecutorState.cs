namespace CodeRunner.Executors
{
    public enum ExecutorState
    {
        Pending,
        Running,
        Ended,
        OutOfMemory,
        OutOfTime,
    }
}
