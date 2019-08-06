using CodeRunner.Helpers;
using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
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
                argOperator.AddSuggestionSource(text =>
                {
                    return Program.Workspace.Operations.Settings.Result?.Items?.Keys ?? Array.Empty<string>();
                });
                res.AddArgument(argOperator);
            }

            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            ResolveContext resolveContext = new ResolveContext();
            AppSettings settings = (await Program.Workspace.Settings)!;
            resolveContext.WithVariable(Operation.VarShell.Name, settings.DefaultShell);
            if (!console.FillVariables(argument.Operation!.GetVariables(), resolveContext))
                return -1;
            argument.Operation.CommandExecuted += (sender, index, result) =>
            {
                if (result.State != Executors.ExecutorState.Ended || result.ExitCode != 0)
                {
                    return Task.FromResult(false);
                }

                console.Out.Write(result.Output);
                console.Error.Write(result.Error);

                return Task.FromResult(true);
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
