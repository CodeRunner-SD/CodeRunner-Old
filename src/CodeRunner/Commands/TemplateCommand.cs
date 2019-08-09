using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class TemplateCommand : BaseCommand<TemplateCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("template", "Manage templates.");
            res.AddCommand(new Templates.ListCommand().Build());
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
