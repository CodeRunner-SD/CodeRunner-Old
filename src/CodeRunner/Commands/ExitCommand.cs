using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class ExitCommand : BaseCommand<ExitCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("exit", "Exit CodeRunner.");
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken) => Task.FromResult(ReplCommand.ExitReplCode);

        public class CArgument
        {
        }
    }
}
