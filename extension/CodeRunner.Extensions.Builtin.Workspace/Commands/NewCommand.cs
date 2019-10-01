using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    [Export]
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

        protected override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            ITerminal terminal = console.GetTerminal();
            string template = argument.Template;
            Packagings.Package<BaseTemplate>? tplItem = await workspace.Templates.GetValue(template);
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
                item = await workspace.Create(argument.Name, tpl,
                    (vars, resolveContext) => Utils.ResolveCallback(vars, resolveContext, context, pipeline));
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
