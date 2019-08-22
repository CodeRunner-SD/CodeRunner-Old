using System;

namespace CodeRunner.Loggings
{
    public abstract class LogFilter
    {
        private class FunctionalFilter : LogFilter
        {
            public FunctionalFilter(Func<LogItem, bool> func)
            {
                Func = func;
            }

            private Func<LogItem, bool> Func { get; }

            public override bool Filter(LogItem item)
            {
                return Func(item);
            }
        }

        public abstract bool Filter(LogItem item);

        public static LogFilter Create(Func<LogItem, bool> filter)
        {
            return new FunctionalFilter(filter);
        }
    }
}
