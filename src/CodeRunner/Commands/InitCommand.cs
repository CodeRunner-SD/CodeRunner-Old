using CodeRunner.Managers;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class InitCommand : BaseCommand<InitCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("init", "Initialize code-runner directory.");
            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            var workspace = operation.Services.Get<Workspace>();
            await workspace.Initialize();
            return 0;
        }

        public class CArgument
        {
        }
    }
}
