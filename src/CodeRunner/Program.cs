using CodeRunner.Commands;
using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using System;
using System.CommandLine;
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
        internal static readonly string AppDescription = string.Join(System.Environment.NewLine, "Code Runner, a CLI tool to run code.", "Copyright (c) StardustDL. All rights reserved.", "Open source with Apache License 2.0 on https://github.com/StardustDL/CodeRunner.");

        public static EnvironmentType Environment { get; set; } = EnvironmentType.Production;

        public static TextReader Input { get; set; }

        public static Logger Logger { get; private set; }

        internal const string ReplCommandId = "repl";
        internal const string CliCommandId = "cli";

        internal static bool EnableRepl { get; set; } = false;

        #region pipelines

        private static readonly Action<ServiceScope> config = services =>
        {
            Input = Console.In;
            SystemConsole console = new SystemConsole();
            services.Add<IConsole>(console);
        };

        private static readonly Action<ServiceScope> create_cmd = services =>
        {
            {
                Command replCommand = new ReplCommand().Build();
                services.Add<Command>(replCommand, ReplCommandId);
            }
            {
                Command cliCommand = new CliCommand().Build();
                services.Add<Command>(cliCommand, CliCommandId);
            }
        };

        private static readonly Action<ServiceScope> config_test = services =>
        {
            TestTerminal console = new TestTerminal();
            services.Add<IConsole>(console);
            if (Input == null)
            {
                throw new ArgumentNullException(nameof(Input));
            }
        };

        private static readonly PipelineOperator<string[], int> cli = async (context) =>
        {
            Parser cliCommand = CommandLines.CreateParser(context.Services.Get<Command>(CliCommandId), context);
            IConsole console = context.Services.Get<IConsole>();
            int exitCode = await cliCommand.InvokeAsync(context.Origin, console);
            if (!EnableRepl)
            {
                context.IsEnd = true;
                return exitCode;
            }
            return 0;
        };

        private static readonly PipelineOperator<string[], int> test_view = (context) =>
        {
            IConsole console = context.Services.Get<IConsole>();
            Workspace workspace = context.Services.Get<Workspace>();
            TestView.Console = console;
            TestView.Workspace = workspace;
            context.IgnoreResult = true;
            return Task.FromResult(0);
        };

        private static readonly PipelineOperator<string[], int> repl = async (context) =>
        {
            IConsole console = context.Services.Get<IConsole>();
            Parser replCommand = CommandLines.CreateParser(context.Services.Get<Command>(ReplCommandId), context);
            ITerminal terminal = console.GetTerminal();
            Workspace workspace = context.Services.Get<Workspace>();

            terminal.OutputLine(workspace.PathRoot.FullName);

            while (Prompt(context, terminal) && !console.IsEndOfInput())
            {
                string? line = console.InputLine();
                if (line != null)
                {
                    if (line == "quit")
                    {
                        break;
                    }

                    int exitCode = await replCommand.InvokeAsync(line, console);
                    if (exitCode != 0)
                    {
                        terminal.OutputErrorLine($"Executed with code {exitCode}.");
                    }
                }
            }

            return 0;
        };

        #endregion

        private static bool Prompt(OperationContext context, ITerminal terminal)
        {
            if (context.Services.TryGet<WorkItem>(out WorkItem workItem))
            {
                terminal.Output(workItem.RelativePath);
            }
            terminal.Output("> ");
            return true;
        }

        public static async Task<int> Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Logger = new Logger(nameof(Main), LogLevel.Debug);

            PipelineBuilder<string[], int> builder = new PipelineBuilder<string[], int>();

            builder.Configure(nameof(create_cmd), create_cmd);

            if (Environment == EnvironmentType.Test)
            {
                builder.Configure(nameof(config_test), config_test);
            }
            else
            {
                builder.Configure(nameof(config), config);
            }

            builder.Use(nameof(cli), cli);

            if (Environment == EnvironmentType.Test)
            {
                builder.Use(nameof(test_view), test_view);
            }

            builder.Use(nameof(repl), repl);

            Pipeline<string[], int> pipeline = await builder.Build(args, Logger);
            PipelineResult<int> result = await pipeline.Consume();
            if (result.IsOk)
            {
                return result.Result;
            }
            else
            {
                Console.Error.WriteLine(result.Exception!.ToString());
                return -1;
            }
        }
    }
}