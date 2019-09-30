using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Loggings;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Console.Commands
{
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

    [Export]
    public class DebugCommand : BaseCommand<DebugCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("debug", "Get information for debug.");
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            ILogger logger = pipeline.Services.GetLogger();
            ITerminal terminal = console.GetTerminal();
            {
                terminal.OutputTable(logger.View(),
                    new OutputTableColumnLogLevelView(nameof(LogItem.Level)),
                    new OutputTableColumnStringView<LogItem>(x => x.Scope, nameof(LogItem.Scope)),
                    new OutputTableColumnStringView<LogItem>(x => x.Time.ToString(CultureInfo.InvariantCulture.DateTimeFormat), nameof(LogItem.Time)),
                    new OutputTableColumnStringView<LogItem>(x => x.Content, nameof(LogItem.Content))
                );
            }
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
