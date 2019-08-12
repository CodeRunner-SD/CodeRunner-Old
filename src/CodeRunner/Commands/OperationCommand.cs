using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class OperationCommand : BaseCommand<OperationCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("operation", "Manage operations.");
            res.AddAlias("task");
            res.AddCommand(new Operations.ListCommand().Build());
            res.AddCommand(new Operations.AddCommand().Build());
            res.AddCommand(new Operations.RemoveCommand().Build());
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
