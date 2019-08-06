using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class RunCommand : BaseCommand<RunCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("run", "Run operation.");
            {
                Argument<Operation?> argOperator = new Argument<Operation?>(new TryConvertArgument<Operation?>((SymbolResult symbolResult, out Operation? value) =>
                {
                    value = null;
                    string _operator = symbolResult.Token.Value;
                    OperationItem? tplItem = Program.Workspace.Operations.GetItem(_operator).Result;
                    if (tplItem == null)
                    {
                        symbolResult.ErrorMessage = $"No this operation: {_operator}.";
                        return false;
                    }
                    Operation? tpl = Program.Workspace.Operations.Get(tplItem).Result;
                    if (tpl == null)
                    {
                        symbolResult.ErrorMessage = $"Can not load this operation: {_operator}.";
                        return false;
                    }
                    value = tpl;
                    return true;
                }))
                {
                    Name = nameof(CArgument.Operation),
                    Arity = ArgumentArity.ExactlyOne,
                };
                res.AddArgument(argOperator);
            }

            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            ResolveContext resolveContext = new ResolveContext();
            var settings = (await Program.Workspace.Settings)!;
            resolveContext.WithVariable(Operation.VarShell.Name, settings.DefaultShell);
            foreach (Variable v in argument.Operation!.GetVariables())
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
            argument.Operation.Handler = async (result) =>
            {
                if (result.State != Executors.ExecutorState.Ended)
                    return false;
                if (result.ExitCode != 0)
                    return false;

                foreach(var s in result.Output)
                {
                    console.Out.WriteLine(s);
                }

                foreach (var s in result.Error)
                {
                    console.Error.WriteLine(s);
                }

                return true;
            };
            await argument.Operation.DoResolve(resolveContext);
            return 0;
        }

        public class CArgument
        {
            public Operation? Operation { get; set; } = null;
        }


    }
}
