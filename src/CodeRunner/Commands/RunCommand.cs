using CodeRunner.Helpers;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using CodeRunner.Templates;
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

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            Workspace workspace = operation.Services.Get<Workspace>();
            ITerminal terminal = console.GetTerminal();
            string op = argument.Operation;
            OperationItem? tplItem = await workspace.Operations.Get(op);
            if (tplItem == null)
            {
                terminal.OutputErrorLine($"No this operation: {op}.");
                return 1;
            }
            Operation? tpl = (await tplItem.Value)?.Data;
            if (tpl == null)
            {
                terminal.OutputErrorLine($"Can not load this operation: {op}.");
                return 1;
            }

            ResolveContext resolveContext = new ResolveContext().FromArgumentList(context.ParseResult.UnparsedTokens);
            AppSettings settings = (await workspace.Settings)!;
            resolveContext.WithVariable(Operation.VarShell.Name, settings.DefaultShell);
            {
                if (operation.Services.TryGet<WorkItem>(out WorkItem item))
                {
                    resolveContext.WithVariable(OperationVariables.InputPath.Name, item.RelativePath);
                }
            }
            if (!terminal.FillVariables(tpl!.GetVariables(), resolveContext))
            {
                return -1;
            }

            OperationCommandExecutingHandler executing = new OperationCommandExecutingHandler((sender, index, process, command) =>
            {
                terminal.OutputInformationLine($"({index + 1}/{sender.Items.Count}) {string.Join(' ', command)}");
                process.WorkingDirectory = workspace.PathRoot.FullName;
                return Task.FromResult(true);
            });

            OperationCommandExecutedHandler executed = new OperationCommandExecutedHandler((sender, index, result) =>
            {
                if (!string.IsNullOrEmpty(result.Output) || !string.IsNullOrEmpty(result.Error))
                {
                    terminal.EnsureAtLeft();
                    terminal.OutputLine("-----");
                    if (!string.IsNullOrEmpty(result.Output))
                    {
                        terminal.Output(result.Output);
                    }

                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        terminal.OutputError(result.Error);
                    }

                    terminal.EnsureAtLeft();
                    terminal.OutputLine("-----");
                }

                terminal.Output($"({index + 1}/{sender.Items.Count}) {result.State.ToString()} -> ");

                if (result.ExitCode != 0)
                {
                    terminal.OutputError(result.ExitCode.ToString());
                }
                else
                {
                    terminal.Output(result.ExitCode.ToString());
                }
                terminal.OutputLine(string.Format(" ({0:f2}MB {1:f2}s)", (double)result.MaximumMemory / 1024 / 1024, result.RunningTime.TotalSeconds));
                return Task.FromResult(result.State == Executors.ExecutorState.Ended && result.ExitCode == 0);
            });

            tpl.CommandExecuting += executing;
            tpl.CommandExecuted += executed;

            bool res = await tpl.Resolve(resolveContext);

            tpl.CommandExecuted -= executed;
            tpl.CommandExecuting -= executing;
            return res ? 0 : -1;
        }

        public class CArgument
        {
            public string Operation { get; set; } = "";
        }


    }
}
