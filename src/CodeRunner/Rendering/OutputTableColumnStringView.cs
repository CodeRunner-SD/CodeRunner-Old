using CodeRunner.Loggings;
using System;
using System.CommandLine.Rendering;

namespace CodeRunner.Rendering
{
    public class OutputTableColumnStringView<T> : IOutputTableColumnView<T>
    {
        public OutputTableColumnStringView(Func<T, string> valueFunc, string header)
        {
            ValueFunc = valueFunc;
            Header = header;
        }

        private Func<T, string> ValueFunc { get; }

        private string Header { get; }

        protected string GetValue(T value) => ValueFunc(value);

        public virtual int Measure(T value) => GetValue(value).Length;

        public virtual int MeasureHeader() => Header.Length;

        public virtual void Render(ITerminal terminal, T value, int length) => terminal.Output(GetValue(value).PadRight(length));

        public virtual void RenderHeader(ITerminal terminal, int length) => terminal.OutputEmphasize(Header.PadRight(length));
    }

    internal class OutputTableColumnLogLevelView : OutputTableColumnStringView<LogItem>
    {
        public OutputTableColumnLogLevelView(string header) : base(x => x.Level.ToString(), header)
        {
        }

        public override void Render(ITerminal terminal, LogItem value, int length)
        {
            string levelStr = GetValue(value).PadRight(length);
            switch (value.Level)
            {
                case LogLevel.Information:
                    terminal.OutputInformation(levelStr);
                    break;
                case LogLevel.Warning:
                    terminal.OutputWarning(levelStr);
                    break;
                case LogLevel.Error:
                    terminal.OutputError(levelStr);
                    break;
                case LogLevel.Fatal:
                    terminal.OutputFatal(levelStr);
                    break;
                case LogLevel.Debug:
                    terminal.OutputDebug(levelStr);
                    break;
            }
        }
    }
}
