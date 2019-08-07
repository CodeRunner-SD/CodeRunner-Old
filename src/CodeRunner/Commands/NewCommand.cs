using CodeRunner.Helpers;
using CodeRunner.Managers;
using CodeRunner.Managers.Configurations;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
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
                Argument<string> argTemplate = new Argument<string>()
                {
                    Name = nameof(CArgument.Template),
                    Arity = ArgumentArity.ExactlyOne,
                };
                res.AddArgument(argTemplate);
            }

            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            var workspace = operation.Services.Get<Workspace>();
            var terminal = console.GetTerminal();
            string template = argument.Template;
            TemplateItem? tplItem = await workspace.Templates.GetItem(template);
            if (tplItem == null)
            {
                terminal.OutputErrorLine($"No this template: {template}.");
                return 1;
            }
            BaseTemplate? tpl = await workspace.Templates.Get(tplItem);
            if (tpl == null)
            {
                terminal.OutputErrorLine($"Can not load this template: {template}.");
                return 1;
            }

            ResolveContext resolveContext = new ResolveContext().FromArgumentList(context.ParseResult.UnmatchedTokens);
            resolveContext.WithVariable(DirectoryTemplate.Var.Name, workspace.PathRoot.FullName);
            if (!terminal.FillVariables(tpl!.GetVariables(), resolveContext))
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
