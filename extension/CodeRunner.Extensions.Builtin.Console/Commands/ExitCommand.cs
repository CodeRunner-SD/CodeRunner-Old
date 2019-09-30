using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Console.Commands
{
    [Export]
    public class ExitCommand : BaseCommand<ExitCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("exit", "Exit CodeRunner.");
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            IHost host = pipeline.Services.Get<IHost>();
            host.Shutdown();
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
