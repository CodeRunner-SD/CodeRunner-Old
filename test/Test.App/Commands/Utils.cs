using CodeRunner.IO;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    public class Utils
    {
        public static async Task<PipelineResult<int>> UsePipeline(Func<PipelineContext, Task<int>> func, IConsole console)
        {
            PipelineBuilder<Workspace, int> builder = new PipelineBuilder<Workspace, int>();
            using TempDirectory dir = new TempDirectory();
            Workspace workspace = new Workspace(dir.Directory);
            builder.Configure("before", scope =>
            {
                scope.Add(console);
                scope.Add(workspace);
            });
            builder.Use("main", context => func(context));
            Pipeline<Workspace, int> pipeline = await builder.Build(workspace, new CodeRunner.Loggings.Logger("", CodeRunner.Loggings.LogLevel.Debug));
            return await pipeline.Consume();
        }
    }
}
