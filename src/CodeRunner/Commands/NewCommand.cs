using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class NewCommand : BaseCommand<NewCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("new", "Create new item from template.");
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
                res.AddArgument(argTemplate);
            }

            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            ResolveContext resolveContext = new ResolveContext();
            resolveContext.WithVariable(DirectoryTemplate.Var.Name, Program.Workspace.PathRoot.FullName);
            foreach (Variable v in argument.Template!.GetVariables())
            {
                if (resolveContext.HasVariable(v.Name))
                {
                    continue;
                }

                console.Out.Write($">> {v.Name} ");
                console.Out.Write(v.IsRequired ? "(*)" : $"({v.Default?.ToString()})");
                console.Out.Write(" ");
                string? line = Program.Input.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    resolveContext.WithVariable(v.Name, line);
                }
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
