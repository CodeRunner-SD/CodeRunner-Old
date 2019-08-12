using CodeRunner.Helpers;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class NowCommand : BaseCommand<NowCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("now", "Set current work-item.")
            {
                TreatUnmatchedTokensAsErrors = true
            };
            {
                Argument<FileInfo> arg = new Argument<FileInfo>(nameof(CArgument.File))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.File)}".ToLower(), "Set working directory.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-f");
                res.AddOption(optCommand);
            }
            {
                Argument<DirectoryInfo> arg = new Argument<DirectoryInfo>(nameof(CArgument.Directory))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.Directory)}".ToLower(), "Set working directory.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-d");
                optCommand.AddAlias("--dir");
                res.AddOption(optCommand);
            }
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            Workspace workspace = operation.Services.Get<Workspace>();
            if (argument.File != null)
            {
                argument.File = CommandLines.ResolvePath(workspace, argument.File);
                operation.Services.Replace<WorkItem>(WorkItem.CreateByFile(workspace, argument.File));
            }
            else if (argument.Directory != null)
            {
                argument.Directory = CommandLines.ResolvePath(workspace, argument.Directory);
                operation.Services.Replace<WorkItem>(WorkItem.CreateByDirectory(workspace, argument.Directory));
            }
            else
            {
                operation.Services.Remove<WorkItem>();
            }
            return Task.FromResult(0);
        }

        public class CArgument
        {
            public FileInfo? File { get; set; }

            public DirectoryInfo? Directory { get; set; }
        }
    }
}
