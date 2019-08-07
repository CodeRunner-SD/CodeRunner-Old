using CodeRunner.Commands;
using CodeRunner.Helpers;
using CodeRunner.IO;
using CodeRunner.Loggings;
using CodeRunner.Managers;
using CodeRunner.Pipelines;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner
{
    public enum EnvironmentType
    {
        Production,
        Development,
        Test
    }

    public class Program
    {
        public static EnvironmentType Environment { get; set; } = EnvironmentType.Production;

        public static TextReader Input { get; set; }

        public static Logger Logger { get; private set; }

        internal const string ReplCommandId = "repl";
        internal const string CliCommandId = "cli";

        internal static bool EnableRepl { get; set; } = false;

        static bool Prompt(ITerminal terminal)
        {
            terminal.Output("> ");
            return true;
        }

        public static async Task<int> Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Logger = new Logger();

            var builder = new PipelineBuilder<string[], int>();

            builder.Configure("create-cmd", services =>
             {
                 {
                     var replCommand = new RootCommand("Code-runner")
                     {
                         TreatUnmatchedTokensAsErrors = false,
                     };
                     replCommand.AddCommand(new InitCommand().Build());
                     replCommand.AddCommand(new NewCommand().Build());
                     replCommand.AddCommand(new RunCommand().Build());
                     services.Add<Command>(replCommand, ReplCommandId);
                 }
                 {
                     var cliCommand = new CliCommand().Build();
                     services.Add<Command>(cliCommand, CliCommandId);
                 }
             });

            if (Environment == EnvironmentType.Test)
            {
                builder.Configure("config-test", services =>
                  {
                      var console = new TestTerminal();
                      services.Add<IConsole>(console);
                      if (Input == null)
                          throw new ArgumentNullException(nameof(Input));
                  });
            }
            else
            {
                builder.Configure("config", services =>
                {
                    Input = Console.In;
                    services.Add<IConsole>(new SystemConsole());
                });
            }

            builder
                .Use("cli", async (context) =>
                {
                    var cliCommand = CommandLines.CreateParser(context.Services.Get<Command>(CliCommandId), context);
                    var console = context.Services.Get<IConsole>();
                    int exitCode = await cliCommand.InvokeAsync(args, console);
                    if (!EnableRepl)
                    {
                        context.IsEnd = true;
                        return exitCode;
                    }
                    return 0;
                });

            if (Environment == EnvironmentType.Test)
            {
                builder
                    .Use("test-view", (context) =>
                    {
                        var console = context.Services.Get<IConsole>();
                        var workspace = context.Services.Get<Workspace>();
                        TestView.Console = console;
                        TestView.Workspace = workspace;
                        context.IgnoreResult = true;
                        return Task.FromResult(0);
                    });
            }

            builder.Use("repl", async (context) =>
                {
                    var console = context.Services.Get<IConsole>();
                    var replCommand = CommandLines.CreateParser(context.Services.Get<Command>(ReplCommandId), context);
                    var terminal = console.GetTerminal();
                    var workspace = context.Services.Get<Workspace>();

                    terminal.OutputLine(workspace.PathRoot.FullName);

                    while (Prompt(terminal) && !console.IsEndOfInput())
                    {
                        string? line = console.InputLine();
                        if (line != null)
                        {
                            if (line == "quit")
                            {
                                break;
                            }

                            var exitCode = await replCommand.InvokeAsync(line, console);
                            if (exitCode != 0)
                            {
                                terminal.OutputErrorLine($"Executed with code {exitCode}.");
                            }
                        }
                    }

                    return 0;
                });

            var pipeline = await builder.Build(args, Logger);
            var result = await pipeline.Consume();
            if (result.IsOk)
            {
                return result.Result;
            }
            else
            {
                return -1;
            }
        }
    }
}