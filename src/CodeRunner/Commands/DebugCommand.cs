using CodeRunner.Loggings;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class DebugCommand : BaseCommand<DebugCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("debug", "Get information for debug.");
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            Logger logger = pipeline.Services.GetLogger();
            ITerminal terminal = console.GetTerminal();
            {
                terminal.OutputTable(logger.GetAll(),
                    new OutputTableColumnLogLevelView(nameof(LogItem.Level)),
                    new OutputTableColumnStringView<LogItem>(x => x.Scope, nameof(LogItem.Scope)),
                    new OutputTableColumnStringView<LogItem>(x => x.Time.ToString(), nameof(LogItem.Time)),
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
