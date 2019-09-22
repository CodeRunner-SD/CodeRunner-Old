using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;

namespace CodeRunner.Helpers
{
    public static class CommandLines
    {
        public static Parser CreateDefaultParser(Command command, PipelineContext context) => CreateParserBuilder(command, context)
            .UseDefaults()
            .Build();

        public static CommandLineBuilder CreateParserBuilder(Command command, PipelineContext context) => new CommandLineBuilder(command)
            .UseMiddleware(inv => inv.BindingContext.AddService(typeof(PipelineContext), () => context));

        /*
        private static FileInfo ResolvePath(Workspace workspace, FileInfo file)
        {
            string rel = Path.GetRelativePath(Directory.GetCurrentDirectory(), file.FullName);
            string path = Path.Join(workspace.PathRoot.FullName, rel);
            return new FileInfo(path);
        }

        private static DirectoryInfo ResolvePath(Workspace workspace, DirectoryInfo file)
        {
            string rel = Path.GetRelativePath(Directory.GetCurrentDirectory(), file.FullName);
            string path = Path.Join(workspace.PathRoot.FullName, rel);
            return new DirectoryInfo(path);
        }
        */
    }
}
