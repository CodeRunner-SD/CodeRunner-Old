using CodeRunner.Helpers;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using CodeRunner.Templates;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class NewCommand : BaseCommand<NewCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("new", "Create new item from template.")
            {
                TreatUnmatchedTokensAsErrors = false
            };
            {
                Argument<string> argTemplate = new Argument<string>(nameof(CArgument.Template))
                {
                    Arity = ArgumentArity.ExactlyOne,
                };
                res.AddArgument(argTemplate);
            }

            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            Workspace workspace = operation.Services.GetWorkspace();
            TextReader input = operation.Services.GetInput();
            ITerminal terminal = console.GetTerminal();
            string template = argument.Template;
            TemplateItem? tplItem = await workspace.Templates.Get(template);
            if (tplItem == null)
            {
                terminal.OutputErrorLine($"No this template: {template}.");
                return 1;
            }
            BaseTemplate? tpl = (await tplItem.Value)?.Data;
            if (tpl == null)
            {
                terminal.OutputErrorLine($"Can not load this template: {template}.");
                return 1;
            }

            ResolveContext resolveContext = new ResolveContext().FromArgumentList(context.ParseResult.UnparsedTokens);
            _ = resolveContext.WithVariable(DirectoryTemplate.Var, workspace.PathRoot.FullName);
            if (!terminal.FillVariables(input, tpl.GetVariables(), resolveContext))
            {
                return -1;
            }

            await tpl.DoResolve(resolveContext);
            return 0;
        }

        public class CArgument
        {
            public string Template { get; set; } = "";
        }
    }
}
