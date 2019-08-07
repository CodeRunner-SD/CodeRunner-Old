using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Pipelines;
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

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            var terminal = console.GetTerminal();
            var view = new LogListView(Program.Logger.GetAll());
            var renderer = new ConsoleRenderer(terminal);
            view.Render(renderer, new Region(0, terminal.CursorTop, 200, 16));
            terminal.OutputLine();
            return 0;
        }

        public class CArgument
        {
        }
    }
}
