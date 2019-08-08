using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Pipelines;
using System;
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
            var terminal = console.GetTerminal();
            {
                var items = Program.Logger.GetAll();
                int[] length = new int[4];
                foreach (var v in items)
                {
                    length[0] = Math.Max(length[0], v.Level.ToString().Length);
                    length[1] = Math.Max(length[1], v.Scope.ToString().Length);
                    length[2] = Math.Max(length[2], v.Time.ToString().Length);
                    length[3] = Math.Max(length[3], v.Content.ToString().Length);
                }
                foreach (var v in items)
                {
                    var levelStr = v.Level.ToString().PadRight(length[0]);
                    switch (v.Level)
                    {
                        case LogLevel.Information:
                            terminal.OutputInformation(levelStr);
                            break;
                        case LogLevel.Warning:
                            terminal.OutputWarning(levelStr);
                            break;
                        case LogLevel.Error:
                            terminal.OutputError(levelStr);
                            break;
                        case LogLevel.Fatal:
                            terminal.OutputFatal(levelStr);
                            break;
                        case LogLevel.Debug:
                            terminal.OutputDebug(levelStr);
                            break;
                    }
                    terminal.Output($" {v.Scope.PadRight(length[1])} {v.Time.ToString().PadRight(length[2])} {v.Content.PadRight(length[3])}");
                    terminal.OutputLine();
                }
            }
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
