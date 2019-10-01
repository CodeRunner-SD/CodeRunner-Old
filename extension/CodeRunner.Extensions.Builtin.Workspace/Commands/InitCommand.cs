using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Managements;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    [Export]
    public class InitCommand : BaseCommand<InitCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("init", "Initialize or uninitialize code-runner directory.");
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

        protected override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            if (argument.Delete)
            {
                await workspace.Clear();
            }
            else
            {
                await workspace.Initialize();

                await workspace.Templates.SetValue("c", Resources.Programming.FileTemplates.C);
                await workspace.Templates.SetValue("python", Resources.Programming.FileTemplates.Python);
                await workspace.Templates.SetValue("cpp", Resources.Programming.FileTemplates.Cpp);
                await workspace.Templates.SetValue("csharp", Resources.Programming.FileTemplates.CSharp);
                await workspace.Templates.SetValue("python", Resources.Programming.FileTemplates.Python);
                await workspace.Templates.SetValue("fsharp", Resources.Programming.FileTemplates.FSharp);
                await workspace.Templates.SetValue("go", Resources.Programming.FileTemplates.Go);
                await workspace.Templates.SetValue("java", Resources.Programming.FileTemplates.Java);
                await workspace.Templates.SetValue("dir", new Package<BaseTemplate>(FileBasedCommandLineOperation.GetDirectoryTemplate()));

                await workspace.Operations.SetValue("c", Resources.Programming.FileOperations.C);
                await workspace.Operations.SetValue("python", Resources.Programming.FileOperations.Python);
                await workspace.Operations.SetValue("cpp", Resources.Programming.FileOperations.Cpp);
                await workspace.Operations.SetValue("python", Resources.Programming.FileOperations.Python);
                await workspace.Operations.SetValue("go", Resources.Programming.FileOperations.Go);
                await workspace.Operations.SetValue("ruby", Resources.Programming.FileOperations.Ruby);
                await workspace.Operations.SetValue("javascript", Resources.Programming.FileOperations.JavaScript);
                await workspace.Operations.SetValue("dir", new Package<BaseOperation>(new FileBasedCommandLineOperation()));
            }
            return 0;
        }

        public class CArgument
        {
            public bool Delete { get; set; } = false;
        }
    }
}
