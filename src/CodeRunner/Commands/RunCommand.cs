using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using CodeRunner.Templates;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class RunCommand : BaseCommand<RunCommand.CArgument>
    {
        private class ConsoleLogger : ILogger
        {
            public ConsoleLogger(ITerminal terminal) => Terminal = terminal;

            public ILogger? Parent { get; }

            private ITerminal Terminal { get; }

            public void Log(LogItem item,
                [CallerMemberName] string memberName = "",
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0) => Terminal.OutputLine(item.Content);

            public ILogger UseFilter(LogFilter filter) => this;

            public IEnumerable<LogItem> View() => Array.Empty<LogItem>();
        }

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

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            Workspace workspace = pipeline.Services.GetWorkspace();
            TextReader input = pipeline.Services.GetInput();
            ITerminal terminal = console.GetTerminal();
            string op = argument.Operation;
            OperationItem? tplItem = await workspace.Operations.Get(op);
            if (tplItem == null)
            {
                terminal.OutputErrorLine($"No this operation: {op}.");
                return 1;
            }
            BaseOperation? tpl = (await tplItem.Value)?.Data;
            if (tpl == null)
            {
                terminal.OutputErrorLine($"Can not load this operation: {op}.");
                return 1;
            }

            ResolveContext resolveContext = new ResolveContext().FromArgumentList(context.ParseResult.UnparsedTokens);
            WorkspaceSettings settings = (await workspace.Settings)!;
            _ = resolveContext
                .SetShell(settings.DefaultShell)
                .SetWorkingDirectory(workspace.PathRoot.FullName);

            {
                WorkItem? workItem = pipeline.Services.GetWorkItem();
                if (workItem != null)
                {
                    _ = resolveContext.SetInputPath(workItem.RelativePath);
                }
            }
            if (!terminal.FillVariables(input, tpl!.GetVariables(), resolveContext))
            {
                return -1;
            }

            /*switch (tpl)
            {
                case SimpleCommandLineOperation clo:
                    {
                        CommandExecutingHandler executing = new CommandExecutingHandler((sender, index, settings, commands) =>
                        {
                            terminal.OutputInformationLine($"({index + 1}/{sender.Items.Count}) {string.Join(' ', commands)}");
                            settings.WorkingDirectory = workspace.PathRoot.FullName;
                            terminal.EnsureAtLeft();
                            terminal.OutputLine("-----");
                            return Task.FromResult(true);
                        });

                        CommandExecutedHandler executed = new CommandExecutedHandler((sender, index, result) =>
                        {
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

                        // clo.CommandExecuting += executing;
                        // clo.CommandExecuted += executed;
                        // clo.CommandExecuted -= executed;
                        // clo.CommandExecuting -= executing;
                        break;
                    }
            }*/
            PipelineBuilder<OperationWatcher, bool> builder = await tpl.Resolve(resolveContext);
            Pipeline<OperationWatcher, bool> opp = await builder.Build(new OperationWatcher(), new ConsoleLogger(terminal));
            PipelineResult<bool> pres = await opp.Consume();
            bool res = pres.IsOk && pres.Result;
            return res ? 0 : -1;
        }

        public class CArgument
        {
            public string Operation { get; set; } = "";
        }
    }
}
