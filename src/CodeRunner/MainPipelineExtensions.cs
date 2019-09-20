using CodeRunner.Commands;
using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Managements.FSBased;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using System;
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
        public static Builder ConfigureReplCommand(this Builder builder) => builder.Configure(nameof(ConfigureReplCommand),
            scope =>
            {
                Command command = new ReplCommand().Build();
                scope.Add<Command>(command, ServicesExtensions.ReplCommandId);
            });

        public static Builder ConfigureCliCommand(this Builder builder) => builder.Configure(nameof(ConfigureCliCommand),
            scope =>
            {
                Command command = new CliCommand().Build();
                scope.Add<Command>(command, ServicesExtensions.CliCommandId);
            });

        public static Builder ConfigureConsole(this Builder builder, IConsole console, TextReader input) => builder.Configure(nameof(ConfigureConsole),
            scope =>
            {
                scope.Add<IConsole>(console);
                scope.Add<TextReader>(input);
            });

        public static Builder ConfigureWorkspace(this Builder builder, IWorkspace workspace) => builder.Configure(nameof(ConfigureWorkspace),
            scope => scope.Add<IWorkspace>(workspace));

        public static Builder ConfigureLogger(this Builder builder, ILogger logger) => builder.Configure(nameof(ConfigureLogger),
            scope => scope.Add<ILogger>(logger));

        public static Builder UseTestView(this Builder builder) => builder.Use(nameof(UseTestView),
            context =>
            {
                TestView.Console = context.Services.GetConsole();
                TestView.Workspace = context.Services.GetWorkspace();
                context.IgnoreResult = true;
                return Task.FromResult(0);
            });

        public static Builder UseCliCommand(this Builder builder) => builder.Use(nameof(UseCliCommand),
            async context =>
            {
                Parser cliCommand = CommandLines.CreateParser(context.Services.GetCliCommand(), context);
                IConsole console = context.Services.GetConsole();
                return await cliCommand.InvokeAsync(context.Origin, console);
            });

        private static bool Prompt(PipelineContext context, ITerminal terminal)
        {
            IWorkItem? workItem = context.Services.GetWorkItem();
            if (workItem != null)
            {
                if (workItem is WorkItem item)
                {
                    if (item.Type == WorkItemType.Directory)
                        terminal.Output("@");
                    terminal.Output(item.RelativePath);
                }
                else
                {
                    terminal.Output(workItem.Name);
                }
            }
            terminal.Output("> ");
            return true;
        }

        public static Builder UseReplCommand(this Builder builder) => builder.Use(nameof(UseReplCommand),
            async context =>
            {
                ITerminal terminal = context.Services.GetConsole().GetTerminal();
                Parser replCommand = CommandLines.CreateParser(context.Services.GetReplCommand(), context);
                TextReader input = context.Services.GetInput();

                terminal.OutputLine(Environment.CurrentDirectory);

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
    }
}