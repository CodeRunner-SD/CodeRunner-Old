using CodeRunner.Loggings;
using System;
using System.Collections.Generic;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Text;

namespace CodeRunner.Helpers
{
    public class LogListView : TableView<LogItem>
    {
        public LogListView(IReadOnlyList<LogItem> items)
        {
            Items = items;

            AddColumn(p => new LogLevelView(p.Level), new TableHeaderView(nameof(LogItem.Level)));
            AddColumn(p => p.Scope, new TableHeaderView(nameof(LogItem.Scope)));
            AddColumn(p => p.Time, new TableHeaderView(nameof(LogItem.Time)));
            AddColumn(p => p.Content,new TableHeaderView(nameof(LogItem.Content)));
        }
    }
}
