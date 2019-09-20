using CodeRunner.Managements;
using CodeRunner.Managements.FSBased;
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

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            IWorkspace workspace = operation.Services.GetWorkspace();
            if (argument.File != null)
            {
                IWorkItem? res = await workspace.Create("", null, (vars, context) =>
                 {
                     _ = context.WithVariable<FileSystemInfo>(RegisterWorkItemTemplate.Target, argument.File);
                     return Task.CompletedTask;
                 });
                if (res != null)
                {
                    operation.Services.Replace<IWorkItem>(res);
                }
            }
            else if (argument.Directory != null)
            {
                IWorkItem? res = await workspace.Create("", null, (vars, context) =>
                {
                    _ = context.WithVariable<FileSystemInfo>(RegisterWorkItemTemplate.Target, argument.Directory);
                    return Task.CompletedTask;
                });
                if (res != null)
                {
                    operation.Services.Replace<IWorkItem>(res);
                }
            }
            else
            {
                operation.Services.Remove<WorkItem>();
            }
            return 0;
        }

        public class CArgument
        {
            public FileInfo? File { get; set; }

            public DirectoryInfo? Directory { get; set; }
        }
    }
}
