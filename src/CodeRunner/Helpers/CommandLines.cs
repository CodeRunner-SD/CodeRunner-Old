using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.IO;

namespace CodeRunner.Helpers
{
    public static class CommandLines
    {
        public static Parser CreateParser(Command command, PipelineContext context)
        {
            CommandLineBuilder builder = new CommandLineBuilder(command);
            builder.UseDefaults();
            builder.UseMiddleware(inv =>
            {
                inv.BindingContext.AddService(typeof(PipelineContext), () => context);
            });
            return builder.Build();
        }

        public static FileInfo ResolvePath(Workspace workspace, FileInfo file)
        {
            string rel = Path.GetRelativePath(Directory.GetCurrentDirectory(), file.FullName);
            string path = Path.Join(workspace.PathRoot.FullName, rel);
            return new FileInfo(path);
        }

        public static DirectoryInfo ResolvePath(Workspace workspace, DirectoryInfo file)
        {
            string rel = Path.GetRelativePath(Directory.GetCurrentDirectory(), file.FullName);
            string path = Path.Join(workspace.PathRoot.FullName, rel);
            return new DirectoryInfo(path);
        }
    }
}
