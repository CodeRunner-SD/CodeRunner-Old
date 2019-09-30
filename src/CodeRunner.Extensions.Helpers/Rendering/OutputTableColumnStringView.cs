using System;
using System.CommandLine.Rendering;

namespace CodeRunner.Extensions.Helpers.Rendering
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
}
