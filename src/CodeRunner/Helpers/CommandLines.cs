using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;

namespace CodeRunner.Helpers
{
    public static class CommandLines
    {
        public static Parser CreateParser(Command command, PipelineContext context) => new CommandLineBuilder(command)
            .UseDefaults()
            .UseMiddleware(inv => inv.BindingContext.AddService(typeof(PipelineContext), () => context))
            .Build();

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
