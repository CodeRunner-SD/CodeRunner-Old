using CodeRunner.Loggings;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class DebugCommand : BaseCommand<DebugCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("debug", "Get information for debug.");
            return res;
        }

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            ITerminal terminal = console.GetTerminal();
            {
                LogItem[] items = Program.Logger.GetAll();

                List<(Func<LogItem, int>, Action<ITerminal, LogItem, int>)> funcs = new List<(Func<LogItem, int>, Action<ITerminal, LogItem, int>)>();
                {
                    static void render(ITerminal ter, LogItem source, int len)
                    {
                        string levelStr = source.Level.ToString().PadRight(len);
                        switch (source.Level)
                        {
                            case LogLevel.Information:
                                ter.OutputInformation(levelStr);
                                break;
                            case LogLevel.Warning:
                                ter.OutputWarning(levelStr);
                                break;
                            case LogLevel.Error:
                                ter.OutputError(levelStr);
                                break;
                            case LogLevel.Fatal:
                                ter.OutputFatal(levelStr);
                                break;
                            case LogLevel.Debug:
                                ter.OutputDebug(levelStr);
                                break;
                        }
                    }
                    funcs.Add((source => source.Level.ToString().Length, render));
                }
                {
                    static void render(ITerminal ter, LogItem source, int len)
                    {
                        ter.Output(source.Scope.ToString().PadRight(len));
                    }
                    funcs.Add((source => source.Scope.ToString().Length, render));
                }
                {
                    static void render(ITerminal ter, LogItem source, int len)
                    {
                        ter.Output(source.Time.ToString().PadRight(len));
                    }
                    funcs.Add((source => source.Time.ToString().Length, render));
                }
                {
                    static void render(ITerminal ter, LogItem source, int len)
                    {
                        ter.Output(source.Content.ToString().PadRight(len));
                    }
                    funcs.Add((source => source.Content.ToString().Length, render));
                }
                terminal.OutputTable(items, funcs.ToArray());
            }
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
