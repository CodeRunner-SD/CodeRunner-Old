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
    public class RunCommand : BaseCommand<RunCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("run", "Run operation.")
            {
                TreatUnmatchedTokensAsErrors = false
            };
            {
                Argument<string> argOperator = new Argument<string>()
                {
                    Name = nameof(CArgument.Operation),
                    Arity = ArgumentArity.ExactlyOne,
                };
                res.AddArgument(argOperator);
            }

            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            var workspace = operation.Services.Get<Workspace>();
            var terminal = console.GetTerminal();
            string op = argument.Operation;
            OperationItem? tplItem = await workspace.Operations.GetItem(op);
            if (tplItem == null)
            {
                terminal.OutputErrorLine($"No this operation: {op}.");
                return 1;
            }
            Operation? tpl = await workspace.Operations.Get(tplItem);
            if (tpl == null)
            {
                terminal.OutputErrorLine($"Can not load this operation: {op}.");
                return 1;
            }

            ResolveContext resolveContext = new ResolveContext().FromArgumentList(context.ParseResult.UnmatchedTokens);
            AppSettings settings = (await workspace.Settings)!;
            resolveContext.WithVariable(Operation.VarShell.Name, settings.DefaultShell);
            if (!terminal.FillVariables(tpl!.GetVariables(), resolveContext))
            {
                return -1;
            }

            tpl.CommandExecuting += (sender, index, process, command) =>
            {
                terminal.OutputInformationLine($"({index + 1}/{sender.Items.Count}) {command}");
                process.WorkingDirectory = workspace.PathRoot.FullName;
                return Task.FromResult(true);
            };

            tpl.CommandExecuted += (sender, index, result) =>
            {
                if (!string.IsNullOrEmpty(result.Output) || !string.IsNullOrEmpty(result.Error))
                {
                    terminal.OutputLine("-----");
                    terminal.Output(result.Output);
                    terminal.OutputError(result.Error);
                    terminal.OutputLine("-----");
                }

                if (result.State != Executors.ExecutorState.Ended || result.ExitCode != 0)
                {
                    terminal.OutputErrorLine($"({index + 1}/{sender.Items.Count}) Exited with {result.ExitCode}.");
                    return Task.FromResult(false);
                }
                terminal.OutputLine($"({index + 1}/{sender.Items.Count}) Exited with {result.ExitCode}.");
                return Task.FromResult(true);
            };
            await tpl.DoResolve(resolveContext);
            return 0;
        }

        public class CArgument
        {
            public string Operation { get; set; } = "";
        }


    }
}
