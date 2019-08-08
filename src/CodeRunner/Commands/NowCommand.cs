﻿using CodeRunner.Helpers;
using CodeRunner.Managers;
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
                var arg = new Argument<FileInfo>(nameof(CArgument.File))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                var optCommand = new Option($"--{nameof(CArgument.File)}".ToLower(), "Set working directory.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-f");
                res.AddOption(optCommand);
            }
            {
                var arg = new Argument<DirectoryInfo>(nameof(CArgument.Directory))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                var optCommand = new Option($"--{nameof(CArgument.Directory)}".ToLower(), "Set working directory.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-d");
                optCommand.AddAlias("--dir");
                res.AddOption(optCommand);
            }
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            var workspace = operation.Services.Get<Workspace>();
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
