using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    [Export]
    public class TemplateCommand : BaseCommand<TemplateCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("template", "Manage templates.");
            res.AddCommand(new Templates.ListCommand().Build());
            res.AddCommand(new Templates.AddCommand().Build());
            res.AddCommand(new Templates.RemoveCommand().Build());
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken) => Task.FromResult(0);

        public class CArgument
        {
        }
    }
}
