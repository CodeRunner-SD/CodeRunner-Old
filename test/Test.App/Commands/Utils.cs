using CodeRunner;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.IO;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    public static class Utils
    {
        public static readonly PipelineOperation<string[], Wrapper<int>> InitializeWorkspace = async context =>
        {
            IWorkspace workspace = context.Services.GetWorkspace();
            await workspace.Initialize();
            return 0;
        };

        public static async Task<PipelineResult<Wrapper<int>>> UseSampleCommandInvoker(IWorkspace workspace, Command command, string[] origin, string input = "", PipelineOperation<string[], Wrapper<int>>? before = null, PipelineOperation<string[], Wrapper<int>>? after = null)
        {
            using MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input));
            using StreamReader sr = new StreamReader(ms);
            PipelineBuilder<string[], Wrapper<int>> builder = CreatePipelineBuilder(new TestTerminal(), sr, workspace);
            if (before != null)
            {
                _ = builder.Use("before", async context =>
                  {
                      _ = await before(context);
                      return context.IgnoreResult();
                  });
            }
            _ = builder.Use("main", async context =>
              {
                  Parser parser = CommandLines.CreateDefaultParser(command, context);
                  return await parser.InvokeAsync(context.Origin, context.Services.GetConsole());
              });
            if (after != null)
            {
                _ = builder.Use("after", async context =>
                  {
                      _ = await after(context);
                      return context.IgnoreResult();
                  });
            }
            return await ConsumePipelineBuilder(builder, new Logger(), origin);
        }

        public static PipelineBuilder<string[], Wrapper<int>> CreatePipelineBuilder(IConsole console, TextReader input, IWorkspace? workspace)
        {
            PipelineBuilder<string[], Wrapper<int>> builder = new PipelineBuilder<string[], Wrapper<int>>()
                .ConfigureConsole(console, input);
            if (workspace != null)
            {
                _ = builder.ConfigureWorkspace(workspace);
            }

            return builder;
        }

        public static async Task<PipelineResult<Wrapper<int>>> ConsumePipelineBuilder(PipelineBuilder<string[], Wrapper<int>> builder, Logger logger, string[] origin)
        {
            Pipeline<string[], Wrapper<int>> pipeline = await builder
                .ConfigureLogger(logger)
                .Build(origin, logger);
            return await pipeline.Consume();
        }
    }
}
