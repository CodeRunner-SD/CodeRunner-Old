using CodeRunner.Managements;
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

                await workspace.Templates.Install("c", CodeRunner.Resources.Programming.Templates.C);
                await workspace.Templates.Install("python", CodeRunner.Resources.Programming.Templates.Python);
                await workspace.Templates.Install("cpp", CodeRunner.Resources.Programming.Templates.Cpp);
                await workspace.Templates.Install("csharp", CodeRunner.Resources.Programming.Templates.CSharp);
                await workspace.Templates.Install("python", CodeRunner.Resources.Programming.Templates.Python);
                await workspace.Templates.Install("fsharp", CodeRunner.Resources.Programming.Templates.FSharp);
                await workspace.Templates.Install("go", CodeRunner.Resources.Programming.Templates.Go);
                await workspace.Templates.Install("java", CodeRunner.Resources.Programming.Templates.Java);

                await workspace.Operations.Install("c", CodeRunner.Resources.Programming.Operations.C);
                await workspace.Operations.Install("python", CodeRunner.Resources.Programming.Operations.Python);
                await workspace.Operations.Install("cpp", CodeRunner.Resources.Programming.Operations.Cpp);
                await workspace.Operations.Install("python", CodeRunner.Resources.Programming.Operations.Python);
                await workspace.Operations.Install("go", CodeRunner.Resources.Programming.Operations.Go);
                await workspace.Operations.Install("ruby", CodeRunner.Resources.Programming.Operations.Ruby);
                await workspace.Operations.Install("javascript", CodeRunner.Resources.Programming.Operations.JavaScript);
            }
            return 0;
        }

        public class CArgument
        {
            public bool Delete { get; set; } = false;
        }
    }
}
