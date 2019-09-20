using CodeRunner.Helpers;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using CodeRunner.Templates;
using System;
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
            {
                Argument<string> argTemplate = new Argument<string>(nameof(CArgument.Name))
                {
                    Arity = ArgumentArity.ExactlyOne,
                };
                res.AddArgument(argTemplate);
            }
            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            IWorkspace workspace = operation.Services.GetWorkspace();
            TextReader input = operation.Services.GetInput();
            ITerminal terminal = console.GetTerminal();
            string template = argument.Template;
            Packagings.Package<BaseTemplate>? tplItem = await workspace.Templates.Get(template);
            if (tplItem == null)
            {
                terminal.OutputErrorLine($"No this template: {template}.");
                return 1;
            }
            BaseTemplate? tpl = tplItem.Data;
            if (tpl == null)
            {
                terminal.OutputErrorLine($"Can not load this template: {template}.");
                return 1;
            }

            IWorkItem? item = null;
            try
            {
                item = await workspace.Create(argument.Name, tpl, (vars, resolveContext) =>
                {
                    _ = resolveContext.FromArgumentList(context.ParseResult.UnparsedTokens);
                    if (!terminal.FillVariables(input, tpl.GetVariables(), resolveContext))
                        throw new ArgumentException();
                    return Task.CompletedTask;
                });
            }
            catch (ArgumentException)
            {
                return -1;
            }
            return 0;
        }

        public class CArgument
        {
            public string Template { get; set; } = "";

            public string Name { get; set; } = "";
        }
    }
}
