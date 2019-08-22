using System.Collections.Generic;
using System.Linq;

namespace CodeRunner.Loggings
{
    public class Logger : ILogger
    {
        public Logger(ILogger? parent = null)
        {
            Parent = parent;
        }

        public ILogger? Parent { get; }

        private List<LogItem> Contents { get; } = new List<LogItem>();

        private List<LogFilter> Filters { get; } = new List<LogFilter>();

        public void Log(LogItem item)
        {
            foreach (LogFilter v in Filters)
            {
                if (!v.Filter(item))
                {
                    return;
                }
            }

            Contents.Add(item);
            if (Parent != null)
            {
                Parent.Log(item);
            }
        }

        public ILogger UseFilter(LogFilter filter)
        {
            Filters.Add(filter);
            return this;
        }

        public IEnumerable<LogItem> View()
        {
            return Contents.AsEnumerable();
        }
    }
}
