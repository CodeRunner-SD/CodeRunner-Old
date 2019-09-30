using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Console.Commands
{
    [Export]
    public class ClearCommand : BaseCommand<ClearCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("clear", "Clear screen.");
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            ITerminal terminal = console.GetTerminal();
            terminal.Clear();
            terminal.SetCursorPosition(0, 0);
            if (console is SystemConsole)
            {
                System.Console.Clear();
                System.Console.SetCursorPosition(0, 0);
            }
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
