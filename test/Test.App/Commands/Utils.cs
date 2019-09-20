using CodeRunner;
using CodeRunner.Helpers;
using CodeRunner.IO;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Managements.FSBased;
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
        public static readonly PipelineOperation<string[], int> InitializeWorkspace = async context =>
        {
            IWorkspace workspace = context.Services.GetWorkspace();
            await workspace.Initialize();
            return 0;
        };

        public static async Task<PipelineResult<int>> UseSampleCommandInvoker(Command command, string[] origin, string input = "", PipelineOperation<string[], int>? before = null, PipelineOperation<string[], int>? after = null)
        {
            using TempDirectory dir = new TempDirectory();
            IWorkspace workspace = new Workspace(dir.Directory);
            using MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input));
            using StreamReader sr = new StreamReader(ms);
            PipelineBuilder<string[], int> builder = CreatePipelineBuilder(new TestTerminal(), sr, workspace);
            if (before != null)
            {
                _ = builder.Use("before", async context =>
                  {
                      _ = await before(context);
                      context.IgnoreResult = true;
                      return 0;
                  });
            }
            _ = builder.Use("main", async context =>
              {
                  Parser parser = CommandLines.CreateParser(command, context);
                  return await parser.InvokeAsync(context.Origin, context.Services.GetConsole());
              });
            if (after != null)
            {
                _ = builder.Use("after", async context =>
                  {
                      _ = await after(context);
                      context.IgnoreResult = true;
                      return 0;
                  });
            }
            return await ConsumePipelineBuilder(builder, new Logger(), origin);
        }

        public static PipelineBuilder<string[], int> CreatePipelineBuilder(IConsole console, TextReader input, IWorkspace? workspace)
        {
            PipelineBuilder<string[], int> builder = new PipelineBuilder<string[], int>()
                .ConfigureConsole(console, input);
            if (workspace != null)
            {
                _ = builder.ConfigureWorkspace(workspace);
            }

            return builder;
        }

        public static async Task<PipelineResult<int>> ConsumePipelineBuilder(PipelineBuilder<string[], int> builder, Logger logger, string[] origin)
        {
            Pipeline<string[], int> pipeline = await builder
                .ConfigureLogger(logger)
                .Build(origin, logger);
            return await pipeline.Consume();
        }
    }
}
