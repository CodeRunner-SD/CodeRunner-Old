using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    [Export]
    public class NowCommand : BaseCommand<NowCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("now", "Set current work-item.")
            {
                TreatUnmatchedTokensAsErrors = true
            };
            {
                Argument<bool> arg = new Argument<bool>(nameof(CArgument.Clear), false)
                {
                    Arity = ArgumentArity.ZeroOrOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.Clear)}".ToLower(), "Clear current work-item.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-c");
                res.AddOption(optCommand);
            }
            return res;
        }

        protected override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            ITerminal terminal = console.GetTerminal();
            if (!argument.Clear)
            {
                IWorkItem? res = await workspace.Create("", null,
                    (vars, resolveContext) => Utils.ResolveCallback(vars, resolveContext, context, pipeline));
                if (res != null)
                {
                    pipeline.Services.Replace<IWorkItem>(res);
                }
                else
                {
                    terminal.OutputErrorLine("Create work-item failed.");
                    return -1;
                }
            }
            else
            {
                pipeline.Services.Remove<IWorkItem>();
            }
            return 0;
        }

        public class CArgument
        {
            public bool Clear { get; set; }
        }
    }
}
