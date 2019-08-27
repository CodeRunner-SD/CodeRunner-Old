using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Pipelines;
using System;
using System.CommandLine;
using System.CommandLine.Rendering;
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

    public static class Program
    {
        internal static readonly string AppDescription = string.Join(System.Environment.NewLine, 
            "Code Runner, a CLI tool to run code.", 
            "Copyright (c) StardustDL. All rights reserved.", 
            "Open source with Apache License 2.0 on https://github.com/StardustDL/CodeRunner.");

        public static EnvironmentType Environment { get; set; } = EnvironmentType.Production;

        public static async Task<int> Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            ILogger logger = new Logger();

            PipelineBuilder<string[], int> builder = new PipelineBuilder<string[], int>();

            builder.ConfigureLogger(logger)
                .ConfigureCliCommand()
                .ConfigureReplCommand();

            if (Environment == EnvironmentType.Test)
            {
                if (TestView.Input == null)
                {
                    throw new NullReferenceException(nameof(TestView.Input));
                }

                builder.ConfigureConsole(new TestTerminal(), TestView.Input);
            }
            else
            {
                builder.ConfigureConsole(new SystemConsole(), Console.In);
            }

            builder.UseCliCommand();

            if (Environment == EnvironmentType.Test)
            {
                builder.UseTestView();
            }

            builder.UseReplCommand();

            Pipeline<string[], int> pipeline = await builder.Build(args, logger);
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