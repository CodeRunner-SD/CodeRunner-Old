﻿using CodeRunner.Managements;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
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

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            Workspace workspace = pipeline.Services.GetWorkspace();
            if (argument.Delete)
            {
                await workspace.Clear();
            }
            else
            {
                await workspace.Initialize();

                await workspace.Templates.Install("c", Resources.Programming.Templates.C);
                await workspace.Templates.Install("python", Resources.Programming.Templates.Python);
                await workspace.Templates.Install("cpp", Resources.Programming.Templates.Cpp);
                await workspace.Templates.Install("csharp", Resources.Programming.Templates.CSharp);
                await workspace.Templates.Install("python", Resources.Programming.Templates.Python);
                await workspace.Templates.Install("fsharp", Resources.Programming.Templates.FSharp);
                await workspace.Templates.Install("go", Resources.Programming.Templates.Go);
                await workspace.Templates.Install("java", Resources.Programming.Templates.Java);
                await workspace.Templates.Install("dir", new Package<BaseTemplate>(FileBasedCommandLineOperation.GetDirectoryTemplate()));

                await workspace.Operations.Install("c", Resources.Programming.Operations.C);
                await workspace.Operations.Install("python", Resources.Programming.Operations.Python);
                await workspace.Operations.Install("cpp", Resources.Programming.Operations.Cpp);
                await workspace.Operations.Install("python", Resources.Programming.Operations.Python);
                await workspace.Operations.Install("go", Resources.Programming.Operations.Go);
                await workspace.Operations.Install("ruby", Resources.Programming.Operations.Ruby);
                await workspace.Operations.Install("javascript", Resources.Programming.Operations.JavaScript);
                await workspace.Operations.Install("dir", new Package<BaseOperation>(new FileBasedCommandLineOperation()));
            }
            return 0;
        }

        public class CArgument
        {
            public bool Delete { get; set; } = false;
        }
    }
}
