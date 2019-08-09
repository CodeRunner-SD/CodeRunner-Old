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
            {
                Argument<bool> arg = new Argument<bool>(nameof(CArgument.Delete), false)
                {
                    Arity = ArgumentArity.ZeroOrOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.Delete)}".ToLower(), "Remove all code-runner files.")
                {
                    Argument = arg
                };
                res.AddOption(optCommand);
            }
            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            Workspace workspace = operation.Services.Get<Workspace>();
            if (argument.Delete)
            {
                await workspace.Clear();
            }
            else
            {
                await workspace.Initialize();
            }
            return 0;
        }

        public class CArgument
        {
            public bool Delete { get; set; } = false;
        }
    }
}
