using CodeRunner.Helpers;
using CodeRunner.Managers;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class CliCommand : BaseCommand<CliCommand.CArgument>
    {
        public override Command Configure()
        {
            RootCommand res = new RootCommand("Code-Runner");
            {
                var arg = new Argument<string>(nameof(CArgument.Command))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                var optCommand = new Option($"--{nameof(CArgument.Command)}", "Command to execute.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-c");
                res.AddOption(optCommand);
            }
            {
                var arg = new Argument<DirectoryInfo>(nameof(CArgument.Directory), new DirectoryInfo(Directory.GetCurrentDirectory()))
                {
                    Arity = ArgumentArity.ZeroOrOne
                };
                var optCommand = new Option($"--{nameof(CArgument.Directory)}", "Set working directory.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-d");
                optCommand.AddAlias("--dir");
                res.AddOption(optCommand);
            }
            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            operation.Services.Add(new Workspace(argument.Directory!));

            if (argument.Command != "")
            {
                var repl = CommandLines.CreateParser(operation.Services.Get<Command>(Program.ReplCommandId), operation);
                return await repl.InvokeAsync(argument.Command, console);
            }

            Program.EnableRepl = true;

            return 0;
        }

        public class CArgument
        {
            public string Command { get; set; } = "";

            public DirectoryInfo? Directory { get; set; }
        }
    }
}
