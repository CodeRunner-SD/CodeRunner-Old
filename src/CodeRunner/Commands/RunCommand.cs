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
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class RunCommand : BaseCommand<RunCommand.CArgument>
    {
        class ConsoleLogger : ILogger
        {
            public ConsoleLogger(ITerminal terminal)
            {
                Terminal = terminal;
            }

            public ILogger? Parent { get; }

            ITerminal Terminal { get; }

            public void Log(LogItem item)
            {
                Terminal.OutputLine(item.Content);
            }

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
            resolveContext.WithVariable(OperationVariables.Shell, settings.DefaultShell);
            resolveContext.WithVariable(OperationVariables.WorkingDirectory, workspace.PathRoot.FullName);
            {
                WorkItem? workItem = pipeline.Services.GetWorkItem();
                if (workItem != null)
                {
                    resolveContext.WithVariable(OperationVariables.InputPath, workItem.RelativePath);
                }
            }
            if (!terminal.FillVariables(input, tpl!.GetVariables(), resolveContext))
            {
                return -1;
            }

            bool res = false;

            switch (tpl)
            {
                case SimpleCommandLineOperation clo:
                    {
                        /*CommandExecutingHandler executing = new CommandExecutingHandler((sender, index, settings, commands) =>
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
                        });*/

                        // clo.CommandExecuting += executing;
                        // clo.CommandExecuted += executed;

                        var builder = await clo.Resolve(resolveContext);
                        var opp = await builder.Build(new OperationWatcher(), new ConsoleLogger(terminal));
                        var pres = await opp.Consume();
                        res = pres.IsOk && pres.Result;

                        // clo.CommandExecuted -= executed;
                        // clo.CommandExecuting -= executing;
                        break;
                    }
            }
            return res ? 0 : -1;
        }

        public class CArgument
        {
            public string Operation { get; set; } = "";
        }
    }
}
