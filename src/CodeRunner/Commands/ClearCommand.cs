using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class ClearCommand : BaseCommand<ClearCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("clear", "Clear screen.");
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            var terminal = operation.Services.Get<ITerminal>();
            terminal.Clear();
            terminal.CursorLeft = 0;
            terminal.CursorTop = 0;
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
