using CodeRunner.Helpers;
using CodeRunner.Managers.Configurations;
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
                Argument<BaseTemplate?> argTemplate = new Argument<BaseTemplate?>(new TryConvertArgument<BaseTemplate?>((SymbolResult symbolResult, out BaseTemplate? value) =>
                {
                    value = null;
                    string template = symbolResult.Token.Value;
                    TemplateItem? tplItem = Program.Workspace.Templates.GetItem(template).Result;
                    if (tplItem == null)
                    {
                        symbolResult.ErrorMessage = $"No this template: {template}.";
                        return false;
                    }
                    BaseTemplate? tpl = Program.Workspace.Templates.Get(tplItem).Result;
                    if (tpl == null)
                    {
                        symbolResult.ErrorMessage = $"Can not load this template: {template}.";
                        return false;
                    }
                    value = tpl;
                    return true;
                }))
                {
                    Name = nameof(CArgument.Template),
                    Arity = ArgumentArity.ExactlyOne,
                };
                argTemplate.AddSuggestionSource(text =>
                {
                    return Program.Workspace.Templates.Settings.Result?.Items?.Keys ?? Array.Empty<string>();
                });
                res.AddArgument(argTemplate);
            }

            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            ResolveContext resolveContext = new ResolveContext().FromArgumentList(context.ParseResult.UnmatchedTokens);
            resolveContext.WithVariable(DirectoryTemplate.Var.Name, Program.Workspace.PathRoot.FullName);
            ITerminal terminal = console.GetTerminal();
            if (!terminal.FillVariables(argument.Template!.GetVariables(), resolveContext))
            {
                return -1;
            }

            await argument.Template.DoResolve(resolveContext);
            return 0;
        }

        public class CArgument
        {
            public BaseTemplate? Template { get; set; } = null;
        }
    }
}
