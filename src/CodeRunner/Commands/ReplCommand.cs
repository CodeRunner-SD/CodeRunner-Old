using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class ReplCommand : BaseCommand<ReplCommand.CArgument>
    {
        public const int ExitReplCode = int.MinValue / 2;

        public override Command Configure()
        {
            RootCommand res = new RootCommand(Program.AppDescription);
            res.AddCommand(new InitCommand().Build());
            res.AddCommand(new NewCommand().Build());
            res.AddCommand(new NowCommand().Build());
            res.AddCommand(new RunCommand().Build());
            res.AddCommand(new ClearCommand().Build());
            res.AddCommand(new ExitCommand().Build());
            res.AddCommand(new DebugCommand().Build());
            res.AddCommand(new TemplateCommand().Build());
            res.AddCommand(new OperationCommand().Build());
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken) => Task.FromResult(0);

        public class CArgument
        {
        }
    }
}
