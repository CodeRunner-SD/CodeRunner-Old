using CodeRunner.Commands;
using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.IO;
using System.Threading.Tasks;
using Builder = CodeRunner.Pipelines.PipelineBuilder<string[], int>;

namespace CodeRunner
{
    public static class MainPipelineExtensions
    {
        public static Builder ConfigureReplCommand(this Builder builder)
        {
            builder.Configure(nameof(ConfigureReplCommand), scope =>
            {
                Command command = new ReplCommand().Build();
                scope.Add<Command>(command, ServicesExtensions.ReplCommandId);
            });
            return builder;
        }

        public static Builder ConfigureCliCommand(this Builder builder)
        {
            builder.Configure(nameof(ConfigureCliCommand), scope =>
            {
                Command command = new CliCommand().Build();
                scope.Add<Command>(command, ServicesExtensions.CliCommandId);
            });
            return builder;
        }

        public static Builder ConfigureConsole(this Builder builder, IConsole console, TextReader input)
        {
            builder.Configure(nameof(ConfigureConsole), scope =>
            {
                scope.Add<IConsole>(console);
                scope.Add<TextReader>(input);
            });
            return builder;
        }

        public static Builder ConfigureWorkspace(this Builder builder, Workspace workspace)
        {
            builder.Configure(nameof(ConfigureWorkspace), scope =>
            {
                scope.Add<Workspace>(workspace);
            });
            return builder;
        }

        public static Builder ConfigureLogger(this Builder builder, Logger logger)
        {
            builder.Configure(nameof(ConfigureLogger), scope =>
            {
                scope.Add<Logger>(logger);
            });
            return builder;
        }

        public static Builder UseTestView(this Builder builder)
        {
            builder.Use(nameof(UseTestView), context =>
            {
                TestView.Console = context.Services.Get<IConsole>();
                TestView.Workspace = context.Services.Get<Workspace>();
                context.IgnoreResult = true;
                return Task.FromResult(0);
            });
            return builder;
        }

        public static Builder UseCliCommand(this Builder builder)
        {
            builder.Use(nameof(UseCliCommand), async context =>
            {
                Parser cliCommand = CommandLines.CreateParser(context.Services.GetCliCommand(), context);
                IConsole console = context.Services.GetConsole();
                return await cliCommand.InvokeAsync(context.Origin, console);
            });
            return builder;
        }

        private static bool Prompt(PipelineContext context, ITerminal terminal)
        {
            WorkItem workItem = context.Services.GetWorkItem();
            if (workItem != null)
            {
                terminal.Output(workItem.RelativePath);
            }
            terminal.Output("> ");
            return true;
        }

        public static Builder UseReplCommand(this Builder builder)
        {
            builder.Use(nameof(UseReplCommand), async context =>
            {
                ITerminal terminal = context.Services.GetConsole().GetTerminal();
                Parser replCommand = CommandLines.CreateParser(context.Services.GetReplCommand(), context);
                Workspace workspace = context.Services.GetWorkspace();
                TextReader input = context.Services.GetInput();

                terminal.OutputLine(workspace.PathRoot.FullName);

                while (Prompt(context, terminal) && !input.IsEndOfInput())
                {
                    string? line = input.InputLine();
                    if (line != null)
                    {
                        if (line == "quit")
                        {
                            break;
                        }

                        int exitCode = await replCommand.InvokeAsync(line, terminal);
                        if (exitCode != 0)
                        {
                            terminal.OutputErrorLine($"Executed with code {exitCode}.");
                        }
                    }
                }

                return 0;
            });
            return builder;
        }
    }
}